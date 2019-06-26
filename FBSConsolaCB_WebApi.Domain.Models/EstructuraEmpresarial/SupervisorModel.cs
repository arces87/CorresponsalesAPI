using System;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial
{
    public class SupervisorModel
    {
        public int Id { get; set; }

        public PersonaModel Persona { get; set; }

        public IEnumerable<CorresponsalModel> Corresponsales { get; set; }

        public bool EstaActivo { get; set; }
    }
}
