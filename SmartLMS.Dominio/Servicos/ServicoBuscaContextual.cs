﻿using Carubbi.GenericRepository;
using Carubbi.Utils.Data;
using SmartLMS.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLMS.Dominio.Servicos
{
    public class ServicoBuscaContextual
    {
        private IContexto _contexto;
        private Usuario _usuarioLogado;
        public ServicoBuscaContextual(IContexto contexto, Usuario usuarioLogado)
        {
            _contexto = contexto;
            _usuarioLogado = usuarioLogado;
        }

        public PagedListResult<ResultadoBusca> Pesquisar(string termo, int pagina = 1, int quantidade = 10)
        {
            PagedListResult<ResultadoBusca> resultados = new PagedListResult<ResultadoBusca>();

            List<ResultadoBusca> listaUnificada = new List<ResultadoBusca>();

            listaUnificada.AddRange(Pesquisar<AreaConhecimento>(termo, pagina, quantidade));
            listaUnificada.AddRange(Pesquisar<Assunto>(termo, pagina, quantidade));
            listaUnificada.AddRange(Pesquisar<Curso>(termo, pagina, quantidade));
            listaUnificada.AddRange(Pesquisar<Aula>(termo, pagina, quantidade));
            listaUnificada.AddRange(Pesquisar<Arquivo>(termo, pagina, quantidade));

            resultados.Entities = listaUnificada.OrderBy(x => x.Descricao)
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade);
            resultados.Count = listaUnificada.Count();
            resultados.HasNext = resultados.Count > pagina * quantidade;
            resultados.HasPrevious = pagina > 1;

            return resultados; 
        }

        private List<ResultadoBusca> Pesquisar<T>(string termo, int pagina, int quantidade)
            where T : class, IResultadoBusca
        {
            GenericRepository<T> repo = new GenericRepository<T>(_contexto);
            SearchQuery<T> query = new SearchQuery<T>();
            query.AddFilter(x => x.Ativo == true && x.Nome.Contains(termo));
            query.AddSortCriteria(new DynamicFieldSortCriteria<T>("Nome"));
            query.Take = quantidade;
            query.Skip = (pagina - 1) * quantidade;
            var resultado = repo.Search(query);
            var retorno = new PagedListResult<ResultadoBusca>();
            retorno.Entities = resultado.Entities.ToList().ConvertAll(new Converter<T, ResultadoBusca>(x => ResultadoBusca.Parse<T>(x, _usuarioLogado, _contexto)));
            

            retorno.Count = resultado.Count;
            retorno.HasNext = resultado.HasNext;
            retorno.HasPrevious = resultado.HasPrevious;

            return retorno.Entities.ToList();
        }

     

   

   
    }
}