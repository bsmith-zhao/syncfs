using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.crypt;
using util.ext;

namespace test.demo
{
    //public static class AesEME
    //{
    //    public class Buff
    //    {
    //        public byte[] data;
    //        public int begin;
    //        public int size;

    //        public Buff() { }

    //        public Buff(byte[] data)
    //        {
    //            this.data = data;
    //            this.size = data.Length;
    //        }

    //        public byte this[int index]
    //        {
    //            get => data[begin + index];
    //            set => data[begin + index] = value;
    //        }

    //        public Buff this[int begin, int end]
    //            => new Buff { data = data, begin = begin, size = end-begin};
    //    }

    //    public class AesEcb
    //    {
    //        public byte[] key;

    //        public void Encrypt(Buff dst, Buff src)
    //        {
    //            var cipher = new Buff(key.ecbEnc(src.data, src.begin, src.size));
    //            copy(dst, cipher);
    //        }

    //        public void Decrypt(Buff dst, Buff src)
    //        {
    //            var data = new Buff(key.ecbDec(src.data, src.begin, src.size));
    //            copy(dst, data);
    //        }

    //        public int BlockSize() => 16;
    //    }

    //    public static int len(Buff buff) => buff.size;
    //    public static Buff make(int size) => new Buff(new byte[size]);
    //    public static void copy(Buff dst, Buff src)
    //    {
    //        for(int i = 0; i < src.size; i++)
    //        {
    //            dst[i] = src[i];
    //        }
    //    }

    //    public static void multByTwo(Buff src, Buff dst)
    //    {
    //        if (len(src) != 16)
    //        {
    //            "len must be 16".log();
    //        }

    //        var tmp = make(16);

    //        tmp[0] = (byte)(2 * src[0]);

    //        if (src[15] >= 128)
    //        {
    //            tmp[0] = (byte)(tmp[0] ^ 135);
    //        }
    //        for (var j = 1; j < 16; j++)
    //        {
    //            tmp[j] = (byte)(2 * src[j]);
    //            if (src[j - 1] >= 128)
    //            {
    //                tmp[j] += 1;
    //            }
    //        }
    //        copy(dst, tmp);
    //    }

    //    public static void xorBlocks(Buff dst, Buff in1, Buff in2)
    //    {
    //        if (len(in1) != len(in2))
    //        {
    //            $"len(in1)={len(in1)} is not equal to len(in2)={len(in2)}".log();
    //        }
    //        for (int i = 0; i < in1.size; i++)
    //        {
    //            dst[i] = (byte)(in1[i] ^ in2[i]);
    //        }
    //    }

    //    public static void aesTransform(Buff dst, Buff src, bool encrypt, AesEcb bc)
    //    {
    //        if (encrypt)
    //            bc.Encrypt(dst, src);
    //        else
    //            bc.Decrypt(dst, src);
    //    }

    //    public static Buff[] tabulateL(AesEcb bc, int m)
    //    {
    //        /* set L0 = 2*AESenc(K; 0) */
    //        var eZero = make(16);

    //        var Li = make(16);

    //        bc.Encrypt(Li, eZero);

    //        var LTable = new Buff[m];//make(m);
    //        // Allocate pool once and slice into m pieces in the loop
    //        var pool = make(m * 16);
    //        for (var i = 0; i < m; i++)
    //        {
    //            multByTwo(Li, Li);

    //            LTable[i] = pool[i * 16, (i + 1) * 16];//pool[i * 16 : (i + 1) * 16];
    //            copy(LTable[i], Li);
    //        }
    //        return LTable;
    //    }

    //    public static Buff Transform(AesEcb bc, Buff tweak, Buff inputData, bool encrypt)
    //    {
    //        // In the paper, the tweak is just called "T". Call it the same here to
    //        // make following the paper easy.
    //        var T = tweak;
    //        // In the paper, the plaintext data is called "P" and the ciphertext is
    //        // called "C". Because encryption and decryption are virtually identical,
    //        // we share the code and always call the input data "P" and the output data
    //        // "C", regardless of the direction.
    //        var P = inputData;

    //        if (bc.BlockSize() != 16)
    //        {
    //            "Using a block size other than 16 is not implemented".log();
    //        }
    //        if (len(T) != 16)
    //        {
    //            $"Tweak must be 16 bytes long, is {len(T)}".log();
    //        }
    //        if (len(P) % 16 != 0)
    //        {
    //            "Data P must be a multiple of 16 long, is {len(P)}".log();
    //        }
    //        var m = len(P) / 16;
    //        if (m == 0 || m > 16 * 8)
    //        {
    //            $"EME operates on 1 to {16 * 8} block-cipher blocks, you passed {m}".log();
    //        }

    //        var C = make(len(P));

    //        var LTable = tabulateL(bc, m);

    //        var PPj = make(16);
    //        for (var j = 0; j < m; j++)
    //        {
    //            var Pj = P[j * 16, (j + 1) * 16];
    //            /* PPj = 2**(j-1)*L xor Pj */
    //            xorBlocks(PPj, Pj, LTable[j]);
    //            /* PPPj = AESenc(K; PPj) */
    //            aesTransform(C[j * 16, (j + 1) * 16], PPj, encrypt, bc);
    //        }

    //        /* MP =(xorSum PPPj) xor T */
    //        var MP = make(16);

    //        xorBlocks(MP, C[0, 16], T);
    //        for (var j = 1; j < m; j++)
    //        {
    //            xorBlocks(MP, MP, C[j * 16, (j + 1) * 16]);
    //        }

    //        /* MC = AESenc(K; MP) */
    //        var MC = make(16);

    //        aesTransform(MC, MP, encrypt, bc);

    //        /* M = MP xor MC */
    //        var M = make(16);

    //        xorBlocks(M, MP, MC);

    //        var CCCj = make(16);
    //        for (var j = 1; j < m; j++)
    //        {
    //            multByTwo(M, M);
    //            /* CCCj = 2**(j-1)*M xor PPPj */
    //            xorBlocks(CCCj, C[j * 16, (j + 1) * 16], M);

    //            copy(C[j * 16, (j + 1) * 16], CCCj);
    //        }

    //        /* CCC1 = (xorSum CCCj) xor T xor MC */
    //        var CCC1 = make(16);

    //        xorBlocks(CCC1, MC, T);
    //        for (var j = 1; j < m; j++)
    //        {
    //            xorBlocks(CCC1, CCC1, C[j * 16, (j + 1) * 16]);
    //        }
    //        copy(C[0, 16], CCC1);

    //        for (var j = 0; j < m; j++)
    //        {
    //            /* CCj = AES-enc(K; CCCj) */
    //            aesTransform(C[j * 16, (j + 1) * 16], C[j * 16, (j + 1) * 16], encrypt, bc);
    //            /* Cj = 2**(j-1)*L xor CCj */
    //            xorBlocks(C[j * 16, (j + 1) * 16], C[j * 16, (j + 1) * 16], LTable[j]);
    //        }

    //        return C;
    //    }

    //    public static byte[] emeEnc(this byte[] key, byte[] plain, byte[] iv)
    //    {
    //        var bc = new AesEcb { key = key };
    //        return Transform(bc, new Buff(iv), new Buff(plain), true).data;
    //    }

    //    public static byte[] emeDec(this byte[] key, byte[] cipher, byte[] iv)
    //    {
    //        var bc = new AesEcb { key = key };
    //        return Transform(bc, new Buff(iv), new Buff(cipher), false).data;
    //    }
    //}
}
