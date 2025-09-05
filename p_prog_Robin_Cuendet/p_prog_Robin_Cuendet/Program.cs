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

                //regarde si une touhe est entrée 
                do
                {
                    cki = Console.ReadKey();
                } while (cki == null);

                Console.Clear();

                //écris les instruction du choix de niveaux
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
                        if (level < 1 || level > 4)
                        {
                            Console.WriteLine("Choix invalide. Essai encore");

                        }
                    }
                    catch
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
                tries = 0;
                do
                {
                    Console.Write("Essai {0}/10 :\nEntre 4 chiffres entre 1 et {1} (ex: 1234) : ", tries, maxNum);
                    try
                    {
                        triedNumber = Convert.ToInt32(Console.ReadLine());
                        if (triedNumber > 1000 || triedNumber < 9999)
                        {
                            tries++;
                            won = CheckEnteredNumber(triedNumber, secretCode, tries, level, won);
                        }
                        else
                        {
                            Console.WriteLine("Le nombre ne va pas veuillez réessayer");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Tu n'as pas entré un nombre ! essaie de nouveau");
                    }

                    if (tries >= 10)
                    {
                        Console.WriteLine("vous avez perdu, le code était : {0}{1}{2}{3}", secretCode[0], secretCode[1], secretCode[2], secretCode[3]);
                        break;
                    }
                    else if (won)
                    {
                        Console.WriteLine("Vous avez gagné");
                        break;
                    }

                } while (true);

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

            }

            return code;
        }

        static bool CheckEnteredNumber(int numbers, int[] Code, int tries, byte level, bool won)
        {
            byte[] results = new byte[4];
            int[]allResponses = new int[10];
            char[] digits = new char[4];

            for (int i = 0; i < 4; i++)
            {
                results[i] = 1;
                digits[i] = numbers.ToString()[i];
            }

            for (int i = 0; i < 4; i++)
            {
                if (Code.Contains(digits[i] - '0') && Code[i] != digits[i] - '0')
                {
                    results[i] = 2;
                }
                if (Code[i] == digits[i] - '0')
                {
                    results[i] = 3;
                }
                if (!Code.Contains(digits[i] - '0'))
                {
                    results[i] = 1;
                }
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
                    break;
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
            Console.ForegroundColor = ConsoleColor.Gray;
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
            Console.ForegroundColor = ConsoleColor.Gray;
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
