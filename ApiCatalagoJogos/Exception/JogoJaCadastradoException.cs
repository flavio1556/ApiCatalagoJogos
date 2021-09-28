using System;


namespace ApiCatalagoJogos.Exception
{
    public class JogoJaCadastradoException : System.Exception
    {
        public JogoJaCadastradoException()
            : base("Este já jogo está cadastrado")
        { }
    }
}
