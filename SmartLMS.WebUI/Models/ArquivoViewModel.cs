﻿using SmartLMS.Dominio.Entidades;
using System.Collections.Generic;
using System;

namespace SmartLMS.WebUI.Models
{
    public class ArquivoViewModel
    {
        public string ArquivoFisico { get; private set; }
        public string Nome { get; private set; }

        public static IEnumerable<ArquivoViewModel> FromEntityList(IEnumerable<Arquivo> arquivos)
        {
            foreach (var item in arquivos)
            {
                yield return FromEntity(item);
            }
          
        }

        private static ArquivoViewModel FromEntity(Arquivo item)
        {
            return new ArquivoViewModel
            {
                Nome = item.Nome,
                ArquivoFisico = item.ArquivoFisico
            };
        }
    }
}