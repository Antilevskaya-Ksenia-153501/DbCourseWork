using Microsoft.EntityFrameworkCore;
using MusicService.Models;

namespace MusicService.Data;

public partial class MusicApplicationContext : DbContext
{
    public MusicApplicationContext()
    {
    }

    public MusicApplicationContext(DbContextOptions<MusicApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<Listener> Listeners { get; set; }

    public virtual DbSet<ListenerSubscription> ListenerSubscriptions { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<PlaylistSong> PlaylistSongs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<SongGenre> SongGenres { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=music_application;Username=postgres;Password=7yuffhjgjkdpip79AK19;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("album_pkey");

            entity.ToTable("album");

            entity.HasIndex(e => e.ArtistIdFk, "idx_album_artist");

            entity.HasIndex(e => e.ReleaseDate, "idx_album_date");

            entity.HasIndex(e => e.Title, "idx_album_title");

            entity.Property(e => e.AlbumId).HasColumnName("album_id");
            entity.Property(e => e.ArtistIdFk).HasColumnName("artist_id_fk");
            entity.Property(e => e.CoverImagePath)
                .HasMaxLength(512)
                .HasColumnName("cover_image_path");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.SongCount)
                .HasDefaultValue(0)
                .HasColumnName("song_count");
            entity.Property(e => e.Title)
                .HasMaxLength(70)
                .HasColumnName("title");
            entity.Property(e => e.TotalDuration)
                .HasDefaultValue(0)
                .HasColumnName("total_duration");

            entity.HasOne(d => d.ArtistIdFkNavigation).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistIdFk)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("album_artist_id_fk_fkey");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("artist_pkey");

            entity.ToTable("artist");

            entity.HasIndex(e => e.UserIdFk, "artist_user_id_fk_key").IsUnique();

            entity.HasIndex(e => e.LabelIdFk, "idx_artist_label");

            entity.HasIndex(e => e.Name, "idx_artist_name");

            entity.HasIndex(e => e.UserIdFk, "idx_artist_user_id");

            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.Biography).HasColumnName("biography");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(512)
                .HasColumnName("image_path");
            entity.Property(e => e.LabelIdFk).HasColumnName("label_id_fk");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.UserIdFk).HasColumnName("user_id_fk");

            entity.HasOne(d => d.LabelIdFkNavigation).WithMany(p => p.Artists)
                .HasForeignKey(d => d.LabelIdFk)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("artist_label_id_fk_fkey");

            entity.HasOne(d => d.UserIdFkNavigation).WithOne(p => p.Artist)
                .HasForeignKey<Artist>(d => d.UserIdFk)
                .HasConstraintName("artist_user_id_fk_fkey");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("audit_log_pkey");

            entity.ToTable("audit_log");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.Operation)
                .HasMaxLength(10)
                .HasColumnName("operation");
            entity.Property(e => e.RecordId).HasColumnName("record_id");
            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .HasColumnName("table_name");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("genre_pkey");

            entity.ToTable("genre");

            entity.HasIndex(e => e.Type, "genre_type_key").IsUnique();

            entity.HasIndex(e => e.Type, "idx_genre_type");

            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasKey(e => e.LabelId).HasName("label_pkey");

            entity.ToTable("label");

            entity.HasIndex(e => e.Name, "idx_label_name");

            entity.Property(e => e.LabelId).HasColumnName("label_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Listener>(entity =>
        {
            entity.HasKey(e => e.ListenerId).HasName("listener_pkey");

            entity.ToTable("listener");

            entity.HasIndex(e => e.Email, "idx_listener_emai");

            entity.HasIndex(e => e.UserIdFk, "idx_listener_user_id");

            entity.HasIndex(e => e.UserIdFk, "listener_user_id_fk_key").IsUnique();

            entity.Property(e => e.ListenerId).HasColumnName("listener_id");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("phone_number");
            entity.Property(e => e.UserIdFk).HasColumnName("user_id_fk");

            entity.HasOne(d => d.UserIdFkNavigation).WithOne(p => p.Listener)
                .HasForeignKey<Listener>(d => d.UserIdFk)
                .HasConstraintName("listener_user_id_fk_fkey");
        });

        modelBuilder.Entity<ListenerSubscription>(entity =>
        {
            entity.HasKey(e => e.ListenerSubscriptionId).HasName("listener_subscription_pkey");

            entity.ToTable("listener_subscription");

            entity.HasIndex(e => e.ListenerIdFk, "idx_listener_subscription_listener_id");

            entity.HasIndex(e => e.StartDate, "idx_listener_subscription_start_date");

            entity.HasIndex(e => e.ListenerIdFk, "listener_subscription_listener_id_fk_key").IsUnique();

            entity.Property(e => e.ListenerSubscriptionId).HasColumnName("listener_subscription_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.ListenerIdFk).HasColumnName("listener_id_fk");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.SubscriptionIdFk).HasColumnName("subscription_id_fk");

            entity.HasOne(d => d.ListenerIdFkNavigation).WithOne(p => p.ListenerSubscription)
                .HasForeignKey<ListenerSubscription>(d => d.ListenerIdFk)
                .HasConstraintName("listener_subscription_listener_id_fk_fkey");

            entity.HasOne(d => d.SubscriptionIdFkNavigation).WithMany(p => p.ListenerSubscriptions)
                .HasForeignKey(d => d.SubscriptionIdFk)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("listener_subscription_subscription_id_fk_fkey");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("payment_method_pkey");

            entity.ToTable("payment_method");

            entity.HasIndex(e => e.ListenerIdFk, "idx_payment_method_listener_id");

            entity.HasIndex(e => e.ListenerIdFk, "payment_method_listener_id_fk_key").IsUnique();

            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("card_number");
            entity.Property(e => e.Cvv)
                .HasMaxLength(3)
                .IsFixedLength()
                .HasColumnName("cvv");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.ListenerIdFk).HasColumnName("listener_id_fk");

            entity.HasOne(d => d.ListenerIdFkNavigation).WithOne(p => p.PaymentMethod)
                .HasForeignKey<PaymentMethod>(d => d.ListenerIdFk)
                .HasConstraintName("payment_method_listener_id_fk_fkey");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("playlist_pkey");

            entity.ToTable("playlist");

            entity.HasIndex(e => e.ListenerIdFk, "idx_playlist_listener_id");

            entity.HasIndex(e => e.Title, "idx_playlist_title");

            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");
            entity.Property(e => e.CoverImagePath)
                .HasMaxLength(512)
                .HasColumnName("cover_image_path");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ListenerIdFk).HasColumnName("listener_id_fk");
            entity.Property(e => e.SongCount)
                .HasDefaultValue(0)
                .HasColumnName("song_count");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.TotalDuration)
                .HasDefaultValue(0)
                .HasColumnName("total_duration");

            entity.HasOne(d => d.ListenerIdFkNavigation).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.ListenerIdFk)
                .HasConstraintName("playlist_listener_id_fk_fkey");
        });

        modelBuilder.Entity<PlaylistSong>(entity =>
        {
            entity.HasKey(e => e.PlaylistSong1).HasName("playlist_song_pkey");

            entity.ToTable("playlist_song");

            entity.Property(e => e.PlaylistSong1).HasColumnName("playlist_song");
            entity.Property(e => e.PlaylistIdFk).HasColumnName("playlist_id_fk");
            entity.Property(e => e.SongIdFk).HasColumnName("song_id_fk");

            entity.HasOne(d => d.PlaylistIdFkNavigation).WithMany(p => p.PlaylistSongs)
                .HasForeignKey(d => d.PlaylistIdFk)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("playlist_song_playlist_id_fk_fkey");

            entity.HasOne(d => d.SongIdFkNavigation).WithMany(p => p.PlaylistSongs)
                .HasForeignKey(d => d.SongIdFk)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("playlist_song_song_id_fk_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.RoleName, "role_role_name_key").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.SongId).HasName("song_pkey");

            entity.ToTable("song");

            entity.HasIndex(e => e.AlbumIdFk, "idx_song_album");

            entity.HasIndex(e => e.ArtistIdFk, "idx_song_artist");

            entity.HasIndex(e => e.Title, "idx_song_title");

            entity.Property(e => e.SongId).HasColumnName("song_id");
            entity.Property(e => e.AlbumIdFk).HasColumnName("album_id_fk");
            entity.Property(e => e.ArtistIdFk).HasColumnName("artist_id_fk");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.FilePath)
                .HasMaxLength(512)
                .HasColumnName("file_path");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.AlbumIdFkNavigation).WithMany(p => p.Songs)
                .HasForeignKey(d => d.AlbumIdFk)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("song_album_id_fk_fkey");

            entity.HasOne(d => d.ArtistIdFkNavigation).WithMany(p => p.Songs)
                .HasForeignKey(d => d.ArtistIdFk)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("song_artist_id_fk_fkey");
        });

        modelBuilder.Entity<SongGenre>(entity =>
        {
            entity.HasKey(e => e.SongGenreId).HasName("song_genre_pkey");

            entity.ToTable("song_genre");

            entity.Property(e => e.SongGenreId).HasColumnName("song_genre_id");
            entity.Property(e => e.GenreIdFk).HasColumnName("genre_id_fk");
            entity.Property(e => e.SongIdFk).HasColumnName("song_id_fk");

            entity.HasOne(d => d.GenreIdFkNavigation).WithMany(p => p.SongGenres)
                .HasForeignKey(d => d.GenreIdFk)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("song_genre_genre_id_fk_fkey");

            entity.HasOne(d => d.SongIdFkNavigation).WithMany(p => p.SongGenres)
                .HasForeignKey(d => d.SongIdFk)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("song_genre_song_id_fk_fkey");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("subscription_pkey");

            entity.ToTable("subscription");

            entity.HasIndex(e => e.Name, "idx_subscription_name");

            entity.HasIndex(e => e.Price, "idx_subscription_price");

            entity.Property(e => e.SubscriptionId).HasColumnName("subscription_id");
            entity.Property(e => e.Features).HasColumnName("features");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.RoleIdFk, "idx_user_role_id");

            entity.HasIndex(e => e.Username, "idx_user_username");

            entity.HasIndex(e => e.Username, "user_username_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.RoleIdFk).HasColumnName("role_id_fk");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.RoleIdFkNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleIdFk)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("user_role_id_fk_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
