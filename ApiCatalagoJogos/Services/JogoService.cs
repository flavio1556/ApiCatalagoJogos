using ApiCatalagoJogos.Entities;
using ApiCatalagoJogos.Exception;
using ApiCatalagoJogos.InputModel;
using ApiCatalagoJogos.Repository;
using ApiCatalagoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalagoJogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IjogoRepository _jogoRepository;
        public JogoService(IjogoRepository jogoRepository)
        {
            this._jogoRepository = jogoRepository;
        }
        public async Task Atualizar(Guid id, JogoInputModel jogo)
        {
            var JogoConsulta = await _jogoRepository.Obter(id);

            if (JogoConsulta == null)
                throw new JogoNaoCadastradoException();

            JogoConsulta.Nome = jogo.Nome;
            JogoConsulta.Produtora = jogo.Produtora;
            JogoConsulta.Preco = jogo.Preco;

            await _jogoRepository.Atualizar(JogoConsulta);

        }

        public async Task Atualizar(Guid id, double preco)
        {
            var JogoConsulta = await _jogoRepository.Obter(id);

            if (JogoConsulta == null)
                throw new JogoNaoCadastradoException();

            JogoConsulta.Preco = preco;

            await _jogoRepository.Atualizar(JogoConsulta);
        }

        public async Task<JogoViewModel> Inserir(JogoInputModel jogo)
        {
            var jogoConsulta = await _jogoRepository.Obter(jogo.Nome, jogo.Produtora);

            if (jogoConsulta.Count() > 0)
                throw new JogoJaCadastradoException();

            var jogoInsert = new Jogo
            {
                Id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };

            await _jogoRepository.Inserir(jogoInsert);

            return new JogoViewModel
            {
                Id = jogoInsert.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var jogos = await _jogoRepository.Obter(pagina, quantidade);

            return jogos.Select(jogo => new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            }).ToList();

        }

        public async Task<JogoViewModel> Obter(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);
                if (jogo == null)
                return null;

            return new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task Remover(Guid id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
                throw new JogoNaoCadastradoException();

            await _jogoRepository.Remover(id);
        }

    }
}
