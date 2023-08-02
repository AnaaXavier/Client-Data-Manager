using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace clientes_gerenciador
{
    class Program
    {
        /*
          Author: Ana Xavier
          Description: The initial intention for this algorithm was to learn
          and understand the concepts of data persistence, structs and lists.
          This is an old code I made, but I think it's worth to share it, so
          feel free to take a look around!
         */
        [System.Serializable] 

        struct cliente 
        {
            public string nome;
            public string email;
            public string cpf;

        }
        static List<cliente> clientes = new List<cliente>();

        static void add() // Variables contained in the struct will be called to register informations.
        {
            cliente cadastro = new cliente();
            Console.Write("Informe seu nome: ");
            cadastro.nome = Console.ReadLine();
            Console.Write("Informe seu email: ");
            cadastro.email = Console.ReadLine();
            Console.Write("Insira seu cpf: ");
            cadastro.cpf = Console.ReadLine();

            if (string.IsNullOrEmpty(cadastro.nome + cadastro.email + cadastro.cpf)) // If nothing is written, it'll show an error message.
            {
                Console.Write("\a\nInforme dados válidos para CONTINUAR.");
            }
            else
            {
                clientes.Add(cadastro);
                salvar();
                Console.Write("\n\aCadastro concluído! Aperte ENTER para sair...");
            }
        }
        
        static void del() // The ID of the register will be requested, when found, all information related to that ID is permanently removed.
        {
            Console.Write("Informe o ID desejado: ");
            int remocao_id = int.Parse(Console.ReadLine());

            if (remocao_id >= 0 && remocao_id <= clientes.Count)
            {
                clientes.RemoveAt(remocao_id);
                salvar();
                Console.Write("Remoção concluída com SUCESSO. Pressione ENTER para sair...");
            }
            else
            {
                Console.WriteLine("Não há cadastros do ID informado.\n Pressione ENTER para sair...");
            }
        }

        static void listagem() // It shows the users that are registered.
        {
            Console.WriteLine("Usuários cadastrados:");
            if(clientes.Count > 0)
            {
                int i = 0;

                foreach (cliente cadastro in clientes)
                {
                    Console.WriteLine($"\nID: {i}");
                    Console.WriteLine($"Nome: {cadastro.nome}");
                    Console.WriteLine($"Email: {cadastro.email}");
                    Console.WriteLine($"Cpf: {cadastro.cpf}\n");
                    i++;
                }
            }
            else
            {
                Console.Write("\aNão há usuários cadastrados... Pressione ENTER para sair.");
            }
        }

        static void salvar() // It saves the list and transform into binary data.
        {
            FileStream Stream = new FileStream("Cadastro de clientes", FileMode.OpenOrCreate);
            BinaryFormatter codificador = new BinaryFormatter();

            codificador.Serialize(Stream, clientes);
            Stream.Close();
        }

        static void carregar() // It remembers last save, like a checkpoint, of the binary data.
        {
            FileStream stream = new FileStream("Cadastro de clientes", FileMode.OpenOrCreate);

            try {

                BinaryFormatter codificador = new BinaryFormatter();

                clientes = (List<cliente>)codificador.Deserialize(stream);
                stream.Close();

                if (clientes == null)
                {
                    clientes = new List<cliente>();
                }
            }
            catch(Exception e)
            {
                clientes = new List<cliente>();
            }
            stream.Close();
        }


        enum menu_ { adicionar = 1, listagem, remover, sair }

        static void Main(string[] args)
        {
            carregar();
            bool saair = false;
            while (!saair)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\t\t\t\t\t\tGERENCIADOR DE USUÁRIOS");
                Console.WriteLine("1- adicionar\n2- listagem\n3- remover\n4- sair"); 
                int tecla_user = int.Parse(Console.ReadLine()); 
                menu_ opc = (menu_)tecla_user; // What the user types in is 'translated' for the menu.

                
                switch (opc)
                {
                    case menu_.adicionar:
                        add();
                        break;
                    case menu_.listagem:
                        listagem();
                        break;
                    case menu_.remover:
                        del();
                        break;
                    case menu_.sair:
                        saair = true;
                        Console.Write("Saindo... Aguarde.");
                        break;
                    default:
                        Console.Write("\aNão há resultados desejados encontrados.");
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}