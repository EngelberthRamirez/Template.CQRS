using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationCore.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var basePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var sqlFilePath = Path.Combine(basePath, "scripts", "init.sql");
            var sql = File.ReadAllText(sqlFilePath);
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            var dropProceduresSql = @"
            IF OBJECT_ID('ObtenerProductosPorPrecio', 'P') IS NOT NULL
                DROP PROCEDURE ObtenerProductosPorPrecio;
            IF OBJECT_ID('ObtenerProductoPorId', 'P') IS NOT NULL
                DROP PROCEDURE ObtenerProductoPorId;
            IF OBJECT_ID('ObtenerProductos', 'P') IS NOT NULL
                DROP PROCEDURE ObtenerProductos;
            IF OBJECT_ID('EliminarProductoPorId', 'P') IS NOT NULL
                DROP PROCEDURE EliminarProductoPorId;
            IF OBJECT_ID('ActualizarProducto', 'P') IS NOT NULL
                DROP PROCEDURE ActualizarProducto;
            IF OBJECT_ID('InsertarProducto', 'P') IS NOT NULL
                DROP PROCEDURE InsertarProducto;";

            migrationBuilder.Sql(dropProceduresSql);
        }
    }
}
