using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

public partial class CentroCulturalRegionalTlahuelilpanContext : DbContext
{
    public CentroCulturalRegionalTlahuelilpanContext()
    {
    }

    public CentroCulturalRegionalTlahuelilpanContext(DbContextOptions<CentroCulturalRegionalTlahuelilpanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumnos { get; set; }

    public virtual DbSet<Docente> Docentes { get; set; }

    public virtual DbSet<Expediente> Expedientes { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<ProgresoEstudiantil> ProgresoEstudiantils { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tallere> Talleres { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-E69PLGON; Initial Catalog=DBASE; user id=sa; pwd=soenezg15; DataBase=Centro_Cultural_Regional_Tlahuelilpan; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasIndex(e => new { e.ApellidoPaterno, e.ApellidoMaterno, e.Nombre }, "IDX_Alumnos_Nombre");

            entity.Property(e => e.AlumnoId).HasColumnName("Alumno_ID");
            entity.Property(e => e.AdultoResponsable)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Adulto_Responsable");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Paterno");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Correo_Electronico");
            entity.Property(e => e.Edad).HasComputedColumnSql("(datediff(year,[Fecha_Nacimiento],getdate())-case when dateadd(year,datediff(year,[Fecha_Nacimiento],getdate()),[Fecha_Nacimiento])>getdate() then (1) else (0) end)", false);
            entity.Property(e => e.FechaNacimiento).HasColumnName("Fecha_Nacimiento");
            entity.Property(e => e.Localidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTelefono)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Numero_Telefono");
            entity.Property(e => e.TelefonoResponsable)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Telefono_Responsable");
        });

        modelBuilder.Entity<Docente>(entity =>
        {
            entity.HasIndex(e => new { e.ApellidoPaterno, e.ApellidoMaterno, e.Nombre }, "IDX_Docentes_Nombre");

            entity.Property(e => e.DocenteId).HasColumnName("Docente_ID");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Paterno");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Localidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NumeroContacto)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Numero_Contacto");
        });

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_Actualizar_Documentos_Completos"));
            entity.HasIndex(e => e.DocumentosCompletos, "IDX_Expedientes_Completos").HasFilter("([Documentos_Completos]=(1))");

            entity.HasIndex(e => e.AlumnoId, "UQ_Alumno_Expediente").IsUnique();

            entity.Property(e => e.ExpedienteId).HasColumnName("Expediente_ID");
            entity.Property(e => e.ActaNacimiento)
                .HasDefaultValue(false)
                .HasColumnName("Acta_Nacimiento");
            entity.Property(e => e.AlumnoId).HasColumnName("Alumno_ID");
            entity.Property(e => e.CertificadoMedico)
                .HasDefaultValue(false)
                .HasColumnName("Certificado_Medico");
            entity.Property(e => e.ComprobanteDomicilio)
                .HasDefaultValue(false)
                .HasColumnName("Comprobante_Domicilio");
            entity.Property(e => e.Curp)
                .HasDefaultValue(false)
                .HasColumnName("CURP");
            entity.Property(e => e.DocumentosCompletos)
                .HasDefaultValue(false)
                .HasColumnName("Documentos_Completos");
            entity.Property(e => e.Fotografias).HasDefaultValue(false);
            entity.Property(e => e.Ine)
                .HasDefaultValue(false)
                .HasColumnName("INE");
            entity.Property(e => e.ReciboPago)
                .HasDefaultValue(false)
                .HasColumnName("Recibo_Pago");

            entity.HasOne(d => d.Alumno).WithOne(p => p.Expediente)
                .HasForeignKey<Expediente>(d => d.AlumnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expedientes_Alumnos");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("TR_Actualizar_Estado_Grupo"));

            entity.HasIndex(e => e.Horario, "IDX_Grupos_Horario");

            entity.HasIndex(e => new { e.TallerId, e.NombreGrupo }, "UQ_Taller_Grupo").IsUnique();

            entity.Property(e => e.GrupoId).HasColumnName("Grupo_ID");
            entity.Property(e => e.Aula)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DocenteId).HasColumnName("Docente_ID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("En curso");
            entity.Property(e => e.FechaFin).HasColumnName("Fecha_Fin");
            entity.Property(e => e.FechaInicio).HasColumnName("Fecha_Inicio");
            entity.Property(e => e.Horario)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreGrupo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Nombre_Grupo");
            entity.Property(e => e.TallerId).HasColumnName("Taller_ID");

            entity.HasOne(d => d.Docente).WithMany(p => p.Grupos)
                .HasForeignKey(d => d.DocenteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grupos_Docentes");

            entity.HasOne(d => d.Taller).WithMany(p => p.Grupos)
                .HasForeignKey(d => d.TallerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grupos_Talleres");
        });

        modelBuilder.Entity<ProgresoEstudiantil>(entity =>
        {
            entity.HasKey(e => e.ProgresoId);

            entity.ToTable("Progreso_Estudiantil", tb => tb.HasTrigger("TR_Actualizar_Egresado"));

            entity.HasIndex(e => e.AlumnoId, "IDX_Progreso_Alumno");

            entity.HasIndex(e => e.Estado, "IDX_Progreso_Egresados").HasFilter("([Estado]='Egresado')");

            entity.HasIndex(e => e.GrupoId, "IDX_Progreso_Grupo");

            entity.HasIndex(e => new { e.AlumnoId, e.GrupoId }, "UQ_Alumno_Grupo").IsUnique();

            entity.Property(e => e.ProgresoId).HasColumnName("Progreso_ID");
            entity.Property(e => e.AlumnoId).HasColumnName("Alumno_ID");
            entity.Property(e => e.Asistencia).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Calificacion).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Inscrito");
            entity.Property(e => e.GrupoId).HasColumnName("Grupo_ID");

            entity.HasOne(d => d.Alumno).WithMany(p => p.ProgresoEstudiantils)
                .HasForeignKey(d => d.AlumnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Progreso_Alumnos");

            entity.HasOne(d => d.Grupo).WithMany(p => p.ProgresoEstudiantils)
                .HasForeignKey(d => d.GrupoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Progreso_Grupos");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId);

            entity.HasIndex(e => e.Nombre, "UQ_Nombre_Rol").IsUnique();

            entity.Property(e => e.RolId).HasColumnName("Rol_ID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tallere>(entity =>
        {
            entity.HasKey(e => e.TallerId);

            entity.HasIndex(e => e.NombreTaller, "IDX_Talleres_Nombre");

            entity.HasIndex(e => e.NombreTaller, "UQ_Nombre_Taller").IsUnique();

            entity.Property(e => e.TallerId).HasColumnName("Taller_ID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Duracion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreTaller)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nombre_Taller");
            entity.Property(e => e.Precio)
                .HasDefaultValue(200.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UrlImagen)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("URL_Imagen");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId);

            entity.HasIndex(e => e.NombreUsuario, "UQ_Nombre_Usuario").IsUnique();

            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DocenteId).HasColumnName("Docente_ID");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_Usuario");
            entity.Property(e => e.RolId).HasColumnName("Rol_ID");
            entity.Property(e => e.UsuarioId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Usuario_ID");

            entity.HasOne(d => d.Docente).WithMany()
                .HasForeignKey(d => d.DocenteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Docentes");

            entity.HasOne(d => d.Rol).WithMany()
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
