using FBS.Dominio.Modelos.Filtro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FBS.Dominio.Servicios.Utilidades
{
    public static class Filtro<T>
    {
        private static object ObtenerPropiedad(object elemento, string llave)
        {
            if (llave.Contains("."))
            {
                var _elementos = llave.Split('.');
                object _auxiliar = null;
                PropertyInfo _retorno = null;
                for (int i = 0; i < _elementos.Count(); i++)
                {
                    if (i == _elementos.Count() - 1)
                    {
                        _retorno = _auxiliar.GetType().GetProperty(_elementos[i]);
                    }
                    else
                    {
                        if (_auxiliar == null)
                        {
                            _auxiliar = elemento.GetType().GetProperty(_elementos[i]).GetValue(elemento);
                        }
                        else
                        {
                            _auxiliar = _auxiliar.GetType().GetProperty(_elementos[i]).GetValue(_auxiliar);
                        }
                    }
                }
                if (_retorno != null)
                    return _retorno.GetValue(_auxiliar);
            }
            else
            {
                if (elemento.GetType().GetProperty(llave) != null)
                    return elemento.GetType().GetProperty(llave).GetValue(elemento);

            }
            return null;
        }

        private static bool FiltrarPropiedad(object value, IEnumerable<ModeloFiltro> filtros)
        {
            foreach (var item in filtros)
            {
                var _valor = "";
                try
                {
                    var _propiedad = ObtenerPropiedad(value, item.Llave);
                    _valor = _propiedad.ToString().ToLower();
                }
                catch (Exception)
                {
                    return false;
                }

                switch (item.Operador)
                {
                    case "DATE":
                        var _entradaFecha = new DateTime();
                        var _acomprobarFecha = new DateTime();
                        if (DateTime.TryParse(item.Valor, out _entradaFecha) && DateTime.TryParse(_valor, out _acomprobarFecha) &&
                            _entradaFecha.Date == _acomprobarFecha.Date)
                            return true;
                        break;
                    case "=":
                        if (_valor == item.Valor.ToLower())
                            return true;
                        break;
                    case "LIKE":
                        if (_valor.Contains(item.Valor.ToLower()))
                            return true;
                        break;
                    case "<>":
                        if (_valor != item.Valor.ToLower())
                            return true;
                        break;
                    case ">":
                        var _entradaEntero = 0;
                        var _aComprobarEntero = 0;
                        var _entradaFlotante = 0f;
                        var _aComprobarFlotante = 0f;
                        if (int.TryParse(_valor, out _aComprobarEntero) && int.TryParse(item.Valor, out _entradaEntero) && _aComprobarEntero < _entradaEntero)
                            return true;
                        else if (float.TryParse(_valor, out _aComprobarFlotante) && float.TryParse(item.Valor, out _entradaFlotante) && _aComprobarFlotante < _entradaFlotante)
                            return true;
                        break;
                    case "<":
                        if (int.TryParse(_valor, out _aComprobarEntero) && int.TryParse(item.Valor, out _entradaEntero) && _aComprobarEntero > _entradaEntero)
                            return true;
                        else if (float.TryParse(_valor, out _aComprobarFlotante) && float.TryParse(item.Valor, out _entradaFlotante) && _aComprobarFlotante > _entradaFlotante)
                            return true;
                        break;
                    case ">=":
                        if (int.TryParse(_valor, out _aComprobarEntero) && int.TryParse(item.Valor, out _entradaEntero) && _aComprobarEntero <= _entradaEntero)
                            return true;
                        else if (float.TryParse(_valor, out _aComprobarFlotante) && float.TryParse(item.Valor, out _entradaFlotante) && _aComprobarFlotante <= _entradaFlotante)
                            return true;
                        break;
                    case "<=":
                        if (int.TryParse(_valor, out _aComprobarEntero) && int.TryParse(item.Valor, out _entradaEntero) && _aComprobarEntero >= _entradaEntero)
                            return true;
                        else if (float.TryParse(_valor, out _aComprobarFlotante) && float.TryParse(item.Valor, out _entradaFlotante) && _aComprobarFlotante >= _entradaFlotante)
                            return true;
                        break;
                    default:
                        break;
                }

            }
            return false;

        }

        public static void Filtrar(ref IEnumerable<T> lista, IEnumerable<ModeloFiltro> filtros)
        {
            lista = lista.Where(l => FiltrarPropiedad(l, filtros)).ToList();
        }

        public static void Ordenar(ref IEnumerable<T> lista, IEnumerable<ModeloOrdenamiento> orden)
        {
            IOrderedEnumerable<T> _listaOrdenada = null;
            for (int i = 0; i < orden.Count(); i++)
            {
                string _valor = orden.ElementAt(i).Columna;
                if (i == 0)
                {
                    if (orden.ElementAt(i).TipoOrden)
                        _listaOrdenada = lista.OrderBy(l => ObtenerPropiedad(l, _valor));
                    else
                        _listaOrdenada = lista.OrderByDescending(l => ObtenerPropiedad(l, _valor));
                }
                else
                {
                    if (orden.ElementAt(i).TipoOrden)
                        _listaOrdenada = _listaOrdenada.ThenBy(l => ObtenerPropiedad(l, _valor));
                    else
                        _listaOrdenada = _listaOrdenada.ThenByDescending(l => ObtenerPropiedad(l, _valor));
                }

            }
            if (_listaOrdenada != null)
                lista = _listaOrdenada.ToList();
        }

        public static void ProcesarLista(ref IEnumerable<T> lista, ModeloPaginacion paginacion, ref int totalElementos)
        {
            if (paginacion.Filtros != null && paginacion.Filtros.Count() > 0)
                Filtrar(ref lista, paginacion.Filtros);
            if (paginacion.Ordenamientos != null && paginacion.Ordenamientos.Count() > 0)
                Ordenar(ref lista, paginacion.Ordenamientos);
            totalElementos = lista.Count();
            if (paginacion.Pagina > -1 && paginacion.CantidadElementos > 0)
            {
                lista = lista.Skip(paginacion.Pagina * paginacion.CantidadElementos).Take(paginacion.CantidadElementos);
            }

        }
    }
}
