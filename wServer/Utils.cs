#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

#endregion

namespace wServer
{
    internal static class EnumerableUtils
    {
        public static T RandomElement<T>(this IEnumerable<T> source, Random rng)
        {
            T current = default(T);
            int count = 0;
            foreach (T element in source)
            {
                count++;
                if (rng.Next(count) == 0)
                {
                    current = element;
                }
            }
            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty");
            }
            return current;
        }
    }

    internal static class StringUtils
    {
        public static bool ContainsIgnoreCase(this string self, string val)
        {
            return self.IndexOf(val, StringComparison.InvariantCultureIgnoreCase) != -1;
        }

        public static bool EqualsIgnoreCase(this string self, string val)
        {
            return self.Equals(val, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    internal static class MathsUtils
    {
        public static double Dist(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static double DistSqr(double x1, double y1, double x2, double y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }
    }

    public class RC4
    {
        private readonly RC4Engine rc4;

        public RC4(byte[] key)
        {
            rc4 = new RC4Engine();
            rc4.Init(true, new KeyParameter(key));
        }

        public void Crypt(byte[] buf, int offset, int len) => rc4.ProcessBytes(buf, offset, len, buf, offset);
    }

    public class RSA
    {
        public static readonly RSA Instance =
            new RSA(@"
                -----BEGIN RSA PRIVATE KEY-----
                MIICXAIBAAKBgQCbqweYUxzW0IiCwuBAzx6HtskrhWW+B0iX4LMu2xqRh4gh52HU
                Vu9nNiXso7utTKCv/HNK19v5xoWp3Cne23sicp2oVGgKMFSowBFbtr+fhsq0yHv+
                JxixkL3WLnXcY3xREz7LOzVMoybUCmJzzhnzIsLPiIPdpI1PxFDcnFbdRQIDAQAB
                AoGAbjGLltB+wbGscKPyiu4S9o71qNEtTG9re9eb/7cp/4qpWxanseA4aB90iSb+
                W5a6yNkz4+8Z0J4vUCaBnThQ2Nyoj4B6HUJpih6f9NbcaqTj/8zibr1YyeEzo4rw
                dO1ptPb9y5Pv8DOAInEb3NhqitLBRm1jguxpK9Ybbnob2QECQQDjpAmsqxk2w0Q0
                IgMlx5Cn9uE/iTXaEuqoYRRig2TH7zhzsoll3XfLyuBdkm0tSyUrA4+V7wYXjCoU
                dEEHhzhJAkEArw+gGzbAWHzLwgvBru/WtceSdaT6XPYyp+xssSD0BYIL8xmkIsyS
                0x6Oh99Ec9Ov1M4qGliJlxdZ3vgVyiuVHQJBAJWXh5ADg/c7zIchzsW15jaqgw0Y
                ot3iznfGC/pM9B568rL9IVNifUXb1SNIhRxdpFgm5+WUhIFW55Q3bUCAOJkCQBah
                VnkuIr9Noql7C5apun/VRMGgihzqVrIOhh5/vAvaO+E5N1aoS3KvSI2X9ylh/CDu
                ZdLyDxdRFXUVbPutlqECQE+4PbsqiekYX4BWRTAnOy5Ly+/ivTWOWNJxHicuNu8i
                0zLn+R6ZamUkKcQI5N/91TGJvkIKXRJTYcII+w5gSdw=
                -----END RSA PRIVATE KEY-----"
            );

        /*        
        -----BEGIN PUBLIC KEY-----
        MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCbqweYUxzW0IiCwuBAzx6Htskr
        hWW+B0iX4LMu2xqRh4gh52HUVu9nNiXso7utTKCv/HNK19v5xoWp3Cne23sicp2o
        VGgKMFSowBFbtr+fhsq0yHv+JxixkL3WLnXcY3xREz7LOzVMoybUCmJzzhnzIsLP
        iIPdpI1PxFDcnFbdRQIDAQAB
        -----END PUBLIC KEY-----
        */

        private readonly RsaEngine engine;
        private readonly AsymmetricKeyParameter key;

        private RSA(string privPem)
        {
            key = (new PemReader(new StringReader(privPem.Trim())).ReadObject() as AsymmetricCipherKeyPair).Private;
            engine = new RsaEngine();
            engine.Init(true, key);
        }

        public string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            byte[] dat = Convert.FromBase64String(str);
            Pkcs1Encoding encoding = new Pkcs1Encoding(engine);
            encoding.Init(false, key);
            return Encoding.UTF8.GetString(encoding.ProcessBlock(dat, 0, dat.Length));
        }

        public string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            byte[] dat = Encoding.UTF8.GetBytes(str);
            Pkcs1Encoding encoding = new Pkcs1Encoding(engine);
            encoding.Init(true, key);
            return Convert.ToBase64String(encoding.ProcessBlock(dat, 0, dat.Length));
        }
    }

    public enum PacketID : byte
    {
        FAILURE = 0, //slotid: 1
        CREATE_SUCCESS = 58, //slotid: 2
        CREATE = 48, //slotid: 3
        PLAYERSHOOT = 41, //slotid: 4
        MOVE = 24, //slotid: 5
        PLAYERTEXT = 9, //slotid: 6
        TEXT = 34, //slotid: 7
        SHOOT2 = 1, //slotid: 8
        DAMAGE = 52, //slotid: 9
        UPDATE = 44, //slotid: 10
        UPDATEACK = 96, //slotid: 11
        NOTIFICATION = 20, //slotid: 12
        NEW_TICK = 31, //slotid: 13
        INVSWAP = 64, //slotid: 14
        USEITEM = 3, //slotid: 15
        SHOW_EFFECT = 78, //slotid: 16
        HELLO = 86, //slotid: 17
        GOTO = 92, //slotid: 18
        INVDROP = 97, //slotid: 19
        INVRESULT = 18, //slotid: 20
        RECONNECT = 68, //slotid: 21
        PING = 8, //slotid: 22
        PONG = 83, //slotid: 23
        MAPINFO = 28, //slotid: 24
        LOAD = 63, //slotid: 25
        PIC = 88, //slotid: 26
        SETCONDITION = 36, //slotid: 27
        TELEPORT = 5, //slotid: 28
        USEPORTAL = 23, //slotid: 29
        DEATH = 12, //slotid: 30
        BUY = 77, //slotid: 31
        BUYRESULT = 56, //slotid: 32
        AOE = 7, //slotid: 33
        GROUNDDAMAGE = 84, //slotid: 34
        PLAYERHIT = 37, //slotid: 35
        ENEMYHIT = 94, //slotid: 36
        AOEACK = 89, //slotid: 37
        SHOOTACK = 10, //slotid: 38
        OTHERHIT = 6, //slotid: 39
        SQUAREHIT = 59, //slotid: 40
        GOTOACK = 99, //slotid: 41
        EDITACCOUNTLIST = 87, //slotid: 42
        ACCOUNTLIST = 53, //slotid: 43
        QUESTOBJID = 4, //slotid: 44
        CHOOSENAME = 25, //slotid: 45
        NAMERESULT = 62, //slotid: 46
        CREATEGUILD = 11, //slotid: 47
        CREATEGUILDRESULT = 95, //slotid: 48
        GUILDREMOVE = 75, //slotid: 49
        GUILDINVITE = 85, //slotid: 50
        ALLYSHOOT = 49, //slotid: 51
        SHOOT = 90, //slotid: 52
        REQUESTTRADE = 82, //slotid: 53
        TRADEREQUESTED = 51, //slotid: 54
        TRADESTART = 74, //slotid: 55
        CHANGETRADE = 101, //slotid: 56
        TRADECHANGED = 38, //slotid: 57
        ACCEPTTRADE = 26, //slotid: 58
        CANCELTRADE = 22, //slotid: 59
        TRADEDONE = 35, //slotid: 60
        TRADEACCEPTED = 100, //slotid: 61
        CLIENTSTAT = 57, //slotid: 62
        CHECKCREDITS = 27, //slotid: 63
        ESCAPE = 16, //slotid: 64
        FILE = 33, //slotid: 65
        INVITEDTOGUILD = 14, //slotid: 66
        JOINGUILD = 67, //slotid: 67
        CHANGEGUILDRANK = 81, //slotid: 68
        PLAYSOUND = 17, //slotid: 69
        GLOBAL_NOTIFICATION = 40, //slotid: 70
        RESKIN = 46, //slotid: 71
        PETYARDCOMMAND = 79, //slotid: 72
        PETCOMMAND = 47, //slotid: 73
        UPDATEPET = 39, //slotid: 74
        NEWABILITYUNLOCKED = 76, //slotid: 75
        UPGRADEPETYARDRESULT = 21, //slotid: 76
        EVOLVEPET = 69, //slotid: 77
        REMOVEPET = 50, //slotid: 78
        HATCHEGG = 30, //slotid: 79
        ENTER_ARENA = 45, //slotid: 80
        ARENANEXTWAVE = 65, //slotid: 81
        ARENADEATH = 55, //slotid: 82
        LEAVEARENA = 15, //slotid: 83
        VERIFYEMAILDIALOG = 80, //slotid: 84
        RESKIN2 = 13, //slotid: 85
        PASSWORDPROMPT = 61, //slotid: 86
        VIEWQUESTS = 91, //slotid: 87
        TINKERQUEST = 98, //slotid: 88
        QUESTFETCHRESPONSE = 60, //slotid: 89
        QUESTREDEEMRESPONSE = 93, //slotid: 90
        PET_CHANGE_FORM_MSG = 42,
        KEY_INFO_REQUEST = 66,
        KEY_INFO_RESPONSE = 19
    }
}