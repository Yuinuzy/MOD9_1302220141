using Microsoft.EntityFrameworkCore;
using Model.mahasiswa;

class mahasiswadb : DbContext
{
    public mahasiswadb(DbContextOptions<mahasiswadb> options) // default setting untuk database
        : base(options) { }
    public DbSet<mahasiswa> mhs => Set<mahasiswa>();
}
