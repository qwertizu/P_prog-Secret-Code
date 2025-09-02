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
            byte level = 0;
            int tries = 1, maxNum, triedNumber;
            int[] secretCode = new int[4];


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

            Console.WriteLine("=== SECCRET CODE ===\n");
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
                secretCode = SetCode(maxNum);
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

            Console.WriteLine("=== SECRET CODE NIVEAU {0} ===\nEssais :\n\n", level);
            do
            {
                Console.Write("Essai {0}/10 :\nEntre 4 chiffres entre 1 et {1} (ex: 1234) : ", tries, maxNum);
                try
                {
                    triedNumber = Convert.ToInt32(Console.ReadLine());

                    tries++;
                    if (level == 1 || level == 3)
                    {
                        WriteAnswerLevel1And3(triedNumber, secretCode, tries);
                    }
                    else
                    {
                        WriteAnswerLevel2And4(triedNumber, secretCode);
                    }
                }
                catch
                {
                    Console.WriteLine("Tu n'as pas entré un nombre ! essaie de nouveau");
                }

            } while (tries <= 10);

            Console.Read();



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
        static void WriteAnswerLevel1And3(int numbers, int[] Code, int tries)
        {
            short num = 0;
            bool[] goodNum = new bool[4];
            char[] digits = new char[4];

            for (int i = 0; i < 4; i++)
            {
                digits[i] = numbers.ToString()[num];
                num++;
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Code[j] == digits[i] - '0' && i == j && goodNum[i] == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        goodNum[i] = true;
                        j = 4;
                    }
                    else if (digits[i] - '0' == Code[j] && goodNum[i] == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        j = 4;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                }
                Console.Write("■");
            }
            for (int i = 0; 1 < 4; i++)
            {

            }
        }

        static void WriteAnswerLevel2And4(int numbers, int[] Code)
        {

        }


    }
}
