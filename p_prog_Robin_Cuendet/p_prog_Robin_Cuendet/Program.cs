///ETML
///Auteur : Robin Cuendet
///Date     29.08.2025
///Description : 


using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace p_prog_SecretCode__RobinCuendet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;
            string restart = "o";
            bool won = false;
            byte level = 0;
            int tries = 1, maxNum, triedNumber;
            int[] secretCode = new int[4];

            do
            {

                //écriture tu titre
                Console.WriteLine("╔════════════════════ Robin Cuendet ════════════════════╗");
                Console.WriteLine("║                                                       ║ ");
                Console.WriteLine("║          Bienvenue dans le jeu : Secret Code          ║ ");
                Console.WriteLine("║                                                       ║ ");
                Console.WriteLine("╚═══════════════════════════════════════════════════════╝ ");

                //écriture des règle du jeu
                Console.WriteLine("Un code secret composé de 4 chiffres est généré.\nÀ toi de le découvrire en 10 essais maximum !\n");
                Console.WriteLine("À chaques essai, tu reçois un indice selon le niveau choisi\n\nPour les niveaux 1 et 3 avec indices visibles : ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("■");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" : chiffre bien placé");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("■");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" : chiffre correcte et bien placé\n■ : chiffre bien placé\n");
                Console.WriteLine("Exemple :\ncode secret : 1234 (caché)\n votre essai : 1325\nIndice :");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("■");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("■■");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("■ (1 bien placé, 2 mal placé, 1 absent)\n");
                Console.WriteLine("Pour les niveaux 2 et 4 avec indices discrets : \nExemple :\nCode secret : 5413 (caché)\nVotre essai : 1234 \nIndice :");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("→ 0 bien placé(s), 3 mal placé(s)\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Appuie sur une touche pour commencer...\n");

                do
                {
                    cki = Console.ReadKey();
                } while (cki == null);

                Console.Clear();

                Console.WriteLine("=== SECRET CODE ===\n");
                Console.WriteLine("Choisi un niveau :");
                Console.WriteLine("1. Débutant\t\t(1 à 6, sans doublons, indices visibles");
                Console.WriteLine("2. Intermédiaire\t(1 à 6, sans doublons, indices discrets");
                Console.WriteLine("3. Avancé\t\t(1 à 8, sans doublons, indices visibles");
                Console.WriteLine("4. Expert\t\t(1 à 9, sans doublons, indices discrets\n");

                do
                {
                    Console.Write("Votre choix (1-4)");
                    level = 0;
                    try
                    {
                        level = Convert.ToByte(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Choix invalide. Essai encore");
                    }
                    if (level < 1 || level > 4)
                    {
                        Console.WriteLine("Choix invalide. Essai encore");

                    }


                } while (level == 0);

                Console.Clear();

                if (level < 3)
                {
                    maxNum = 6;
                    SetCodeWithoutRepeatNumbers(secretCode);
                }
                else if (level == 3)
                {
                    maxNum = 8;
                    secretCode = SetCode(maxNum);
                }
                else
                {
                    maxNum = 9;
                    secretCode = SetCode(maxNum);
                }

                Console.Title = "Code Secret : " + secretCode[0] + secretCode[1] + secretCode[2] + secretCode[3];

                Console.WriteLine("=== SECRET CODE NIVEAU {0} ===\nEssais :\n\n", level);
                do
                {
                    Console.Write("Essai {0}/10 :\nEntre 4 chiffres entre 1 et {1} (ex: 1234) : ", tries, maxNum);
                    try
                    {
                        triedNumber = Convert.ToInt32(Console.ReadLine());

                        tries++;
                        won = CheckEnteredNumber(triedNumber, secretCode, tries, level, won);
                    }
                    catch
                    {
                        Console.WriteLine("Tu n'as pas entré un nombre ! essaie de nouveau");
                    }

                } while (tries <= 10 || won == true);

                Console.WriteLine("Voulez-vous recommencer? (o/n)");

                restart = Console.ReadLine();

            }while (restart.ToLower() == "o");

        }
        static int[] SetCode(int numMax)
        {
            int[] code = new int[4];
            Random rnd = new Random();
            for (int i = 0; i <= 3; i++)
            {
                code[i] = rnd.Next(1, numMax);
                Console.Write(code[i]);
            }

            return code;
        }

        static bool CheckEnteredNumber(int numbers, int[] Code, int tries, byte level, bool won)
        {
            short num = 0;
            byte[] results = new byte[4];
            bool[] goodNum = new bool[4];
            char[] digits = new char[4];

            for (int i = 0; i < 4; i++)
            {
                results[i] = 1;
                goodNum[i] = false;
                digits[i] = numbers.ToString()[num];
                num++;
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Code[i] == digits[i] - '0')
                    {
                        results[i] = 3;
                        goodNum[j] = true;
                        j = 4;
                    }
                    else if (digits[i] - '0' == Code[j])
                    {
                        results[i] = 2;
                        j = 4;
                    }
                    else
                    {
                        results[i] = 1;
                    }

                }
                Console.Write(results[i]);
            }

            if (level == 1 || level == 3)
            {
                WriteAnswerLevel1And3(results);
            }
            else
            {
                WriteAnswerLevel2And4(results);
            }
            for (int i = 0;i < 4; i++)
            {
                if (results[i] == 3)
                {
                    won = true;
                }  
                else
                {
                    won = false;
                    i = 4;
                }
            }
            return won;
        }
        static void WriteAnswerLevel1And3(byte[]results)
        {
            for (int i = 0; i < 4; i++)
            {
                if (results[i] == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }else if (results[i] == 2)
                {
                    Console.ForegroundColor= ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.Write('■');
            }
        }

        static void WriteAnswerLevel2And4(byte[] results)
        {
            byte goods = 0, ok = 0,bads = 0;
            for (int i = 0; i < 4; i++)
            {
                if (results[i] == 1)
                {
                    bads++;
                }
                else if (results[i] == 2)
                {
                    ok++;
                }
                else
                {
                    goods++;
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("→ {0} bien placé(s), {1} mal placé(s), {2} fausse(s)", goods, ok, bads);
        }

        static void SetCodeWithoutRepeatNumbers(int[] Code)
        {
            Random rnd = new Random();
            int newDigit;
            for(int i = 0; i < 4; i++)
            {
                do
                {
                    newDigit = rnd.Next(1, 6);
                } while (Array.IndexOf(Code, newDigit, 0, i) >= 0);
                Code[i] = newDigit;
            }
        }


    }
}
