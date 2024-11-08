#region

using log4net;
using System;
using System.Collections.Generic;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.commands
{
    public abstract class Command
    {
        public enum RankName
        {
            PLAYER = 0,

            DONATOR = 10,

            MODERATOR = 30,

            ADMIN = 40,

            OWNER = 50
        }

        protected static readonly ILog log = LogManager.GetLogger(typeof(Command));

        public Command(string name, RankName rank = RankName.PLAYER)
        {
            CommandName = name;
            PermissionLevel = (int)rank;
        }

        public string CommandName { get; }
        public int PermissionLevel { get; }

        protected abstract bool Process(Player player, RealmTime time, string[] args);

        private static int GetPermissionLevel(Player player)
        {
            return player.Client.Account.Rank;
        }

        public bool HasPermission(Player player)
        {
            if (GetPermissionLevel(player) < PermissionLevel)
                return false;
            return true;
        }

        public bool Execute(Player player, RealmTime time, string args)
        {
            if (!HasPermission(player))
            {
                player.SendError("No permission!");
                return false;
            }

            try
            {
                string[] a = args.Split(' ');
                return Process(player, time, a);
            }
            catch (Exception ex)
            {
                // log.Error("Error when executing the command.", ex);
                player.SendError("Error when executing the command.");
                return false;
            }
        }
    }

    public class CommandManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CommandManager));

        private readonly Dictionary<string, Command> cmds;

        private RealmManager manager;

        public CommandManager(RealmManager manager)
        {
            this.manager = manager;
            cmds = new Dictionary<string, Command>(StringComparer.InvariantCultureIgnoreCase);
            Type t = typeof(Command);
            foreach (Type i in t.Assembly.GetTypes())
                if (t.IsAssignableFrom(i) && i != t)
                {
                    Command instance = (Command)Activator.CreateInstance(i);
                    cmds.Add(instance.CommandName, instance);
                }
        }

        public IDictionary<string, Command> Commands
        {
            get { return cmds; }
        }

        public bool Execute(Player player, RealmTime time, string text)
        {
            int index = text.IndexOf(' ');
            string cmd = text.Substring(1, index == -1 ? text.Length - 1 : index - 1);
            string args = index == -1 ? "" : text.Substring(index + 1);

            Command command;
            if (!cmds.TryGetValue(cmd, out command))
            {
                player.SendError("Unknown command!");
                return false;
            }
            log.InfoFormat("[Command] <{0}> {1}", player.Name, text);
            return command.Execute(player, time, args);
        }
    }
}
