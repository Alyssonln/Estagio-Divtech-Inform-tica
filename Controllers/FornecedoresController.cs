using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using FornecedoresApp.Data;
using FornecedoresApp.Models;

namespace FornecedoresApp.Controllers
{
    public class FornecedoresController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FornecedoresController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private string EnsureUploadDir()
        {
            var dir = Path.Combine(_env.WebRootPath, "uploads", "fornecedores");
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static bool IsPng(IFormFile file) =>
            file != null && string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase);

        private static bool TooBig(IFormFile file, long maxBytes = 2 * 1024 * 1024) =>
            file != null && file.Length > maxBytes;

        private void DeletePhysicalFileIfExists(string? webPath)
        {
            if (string.IsNullOrWhiteSpace(webPath)) return;
            var relative = webPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var full = Path.Combine(_env.WebRootPath, relative);
            if (System.IO.File.Exists(full))
            {
                try { System.IO.File.Delete(full); } catch { }
            }
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.Fornecedores.AsNoTracking().ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var fornecedor = await _context.Fornecedores
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fornecedor == null) return NotFound();

            return View(fornecedor);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fornecedor fornecedor, IFormFile? foto)
        {
            if (foto is not null)
            {
                if (!IsPng(foto)) ModelState.AddModelError(nameof(foto), "A foto deve ser um arquivo PNG.");
                if (TooBig(foto)) ModelState.AddModelError(nameof(foto), "Arquivo muito grande (máx. 2MB).");
            }

            if (!ModelState.IsValid) return View(fornecedor);

            if (foto is not null)
            {
                var dir = EnsureUploadDir();
                var fileName = $"{Guid.NewGuid()}.png";
                var filePath = Path.Combine(dir, fileName);
                using var stream = System.IO.File.Create(filePath);
                await foto.CopyToAsync(stream);
                fornecedor.FotoPath = $"/uploads/fornecedores/{fileName}";
            }

            _context.Add(fornecedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null) return NotFound();

            return View(fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Fornecedor fornecedor, IFormFile? foto)
        {
            if (id != fornecedor.Id) return NotFound();

            if (foto is not null)
            {
                if (!IsPng(foto)) ModelState.AddModelError(nameof(foto), "A foto deve ser um arquivo PNG.");
                if (TooBig(foto)) ModelState.AddModelError(nameof(foto), "Arquivo muito grande (máx. 2MB).");
            }

            if (!ModelState.IsValid) return View(fornecedor);

            var existente = await _context.Fornecedores.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            if (existente is null) return NotFound();

            if (foto is not null)
            {
                var dir = EnsureUploadDir();
                var fileName = $"{Guid.NewGuid()}.png";
                var filePath = Path.Combine(dir, fileName);
                using var stream = System.IO.File.Create(filePath);
                await foto.CopyToAsync(stream);

                if (!string.IsNullOrEmpty(existente.FotoPath))
                    DeletePhysicalFileIfExists(existente.FotoPath);

                fornecedor.FotoPath = $"/uploads/fornecedores/{fileName}";
            }
            else
            {
                fornecedor.FotoPath = existente.FotoPath;
            }

            try
            {
                _context.Update(fornecedor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FornecedorExists(fornecedor.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var fornecedor = await _context.Fornecedores
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fornecedor == null) return NotFound();

            return View(fornecedor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor != null)
            {
                if (!string.IsNullOrEmpty(fornecedor.FotoPath))
                    DeletePhysicalFileIfExists(fornecedor.FotoPath);

                _context.Fornecedores.Remove(fornecedor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FornecedorExists(int id) =>
            _context.Fornecedores.Any(e => e.Id == id);
    }
}
