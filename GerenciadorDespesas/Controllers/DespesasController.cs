﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciadorDespesas.Models;
using X.PagedList;
using GerenciadorDespesas.ViewModels;

namespace GerenciadorDespesas.Controllers
{
    public class DespesasController : Controller
    {
        private readonly Contexto _context;

        #region Construtor
        public DespesasController(Contexto context)
        {
            _context = context;
        }
        #endregion


        #region Index
        public async Task<IActionResult> Index(int? pagina)
        {
            const int itensPagina = 10;
            int numeroPagina = (pagina ?? 1);

            ViewData["Meses"] = new SelectList(_context.Meses.Where(x => x.MesId == x.Salarios.MesId), "MesId", "Nome");

            var contexto = _context.Despesas.Include(d => d.Meses).Include(d => d.TipoDespesas).OrderBy(d => d.MesId);
            return View(await contexto.ToPagedListAsync(numeroPagina, itensPagina));
        }
        #endregion

        #region create
        public IActionResult Create()
        {
            ViewData["MesId"] = new SelectList(_context.Meses, "MesId", "Nome");
            ViewData["TipoDespesaId"] = new SelectList(_context.TipoDespesas, "TipoDespesaId", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DespesaId,MesId,TipoDespesaId,Valor")] Despesas despesas)
        {
            if (ModelState.IsValid)
            {
                TempData["Confirmacao"] = "Despesa cadastrada com sucesso.";

                _context.Add(despesas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MesId"] = new SelectList(_context.Meses, "MesId", "Nome", despesas.MesId);
            ViewData["TipoDespesaId"] = new SelectList(_context.TipoDespesas, "TipoDespesaId", "Nome", despesas.TipoDespesaId);
            return View(despesas);
        }
        #endregion

        #region edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var despesas = await _context.Despesas.FindAsync(id);
            if (despesas == null)
            {
                return NotFound();
            }
            ViewData["MesId"] = new SelectList(_context.Meses, "MesId", "Nome", despesas.MesId);
            ViewData["TipoDespesaId"] = new SelectList(_context.TipoDespesas, "TipoDespesaId", "Nome", despesas.TipoDespesaId);
            return View(despesas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DespesaId,MesId,TipoDespesaId,Valor")] Despesas despesas)
        {
            if (id != despesas.DespesaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["Confirmacao"] = "Despesa atualizada com sucesso.";
                    _context.Update(despesas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DespesasExists(despesas.DespesaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MesId"] = new SelectList(_context.Meses, "MesId", "Nome", despesas.MesId);
            ViewData["TipoDespesaId"] = new SelectList(_context.TipoDespesas, "TipoDespesaId", "Nome", despesas.TipoDespesaId);
            return View(despesas);
        }
        #endregion


        #region delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var despesas = await _context.Despesas.FindAsync(id);
            _context.Despesas.Remove(despesas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private bool DespesasExists(int id)
        {
            return _context.Despesas.Any(e => e.DespesaId == id);
        }

        #region métodos para gráficos
        public JsonResult GastosTotaisMes(int mesId)
        {
            GastosTotaisMesViewModel gastos = new GastosTotaisMesViewModel();

            gastos.ValorTotalGasto = _context.Despesas.Where(d => d.Meses.MesId == mesId).Sum(d => d.Valor);
            gastos.Salario = _context.Salarios.Where(s => s.Meses.MesId == mesId).Select(s => s.Valor).FirstOrDefault();

            return Json(gastos);
        }

        public JsonResult GastoMes(int mesId)
        {
            var query = from despesas in _context.Despesas
                        where despesas.Meses.MesId == mesId
                        group despesas by despesas.TipoDespesas.Nome into g
                        select new
                        {
                            TiposDespesas = g.Key,
                            Valores = g.Sum(d => d.Valor)
                        };

            return Json(query);
        }

        public JsonResult GastosTotais()
        {
            var query = _context.Despesas
                .OrderBy(m => m.Meses.MesId)
                .GroupBy(m => m.Meses.MesId)
                .Select(d => new { NomeMeses = d.Select(x => x.Meses.Nome).Distinct(), Valores = d.Sum(x => x.Valor) });

            return Json(query);
        }
        #endregion
    }
}
