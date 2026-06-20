BEGIN TRANSACTION;

CREATE TABLE "Enderecos" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Enderecos" PRIMARY KEY AUTOINCREMENT,
    "UsuarioId" INTEGER NOT NULL,
    "cep" TEXT NOT NULL,
    "logradouro" TEXT NOT NULL,
    "numero" TEXT NOT NULL,
    "complemento" TEXT NULL,
    "bairro" TEXT NOT NULL,
    "cidade" TEXT NOT NULL,
    "uf" TEXT NOT NULL,
    "ibge" TEXT NULL,
    CONSTRAINT "FK_Enderecos_Usuarios_UsuarioId" FOREIGN KEY ("UsuarioId") REFERENCES "Usuarios" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Enderecos_UsuarioId" ON "Enderecos" ("UsuarioId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260620172030_CreateEnderecos', '10.0.9');

COMMIT;
