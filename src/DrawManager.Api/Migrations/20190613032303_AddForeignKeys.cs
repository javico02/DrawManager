using Microsoft.EntityFrameworkCore.Migrations;

namespace DrawManager.Api.Migrations
{
    public partial class AddForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Limitaciones de Sqlite para las migraciones: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations
            // Como resolver las limitaciones para las migraciones: https://sqlite.org/lang_altertable.html#otheralter
            // La eliminación de claves primarias y foráneas no son soportadas por Sqlite, por lo que hay que hacer una actualización manual.
            // Se elimina restricción que puedan ejercer las FK en caso de eliminación o actualización.
            // Se crea una tabla temporal con los datos de la tabla PrizeSelectionSteps.
            // Se elimina la tabla con el schema actual.
            // Se crea una nueva tabla adicionando el nuevo campo y el nuevo PK compuesto por los 3 campos asi como los FKs.
            // Se crean los indices para los campos que son parte de la PK y que son ademas FK.
            // Se copian los datos que existian en la tabla.
            // Se elimina la tabla temporal.
            // Se restituye la restricción de FK en caso de eliminación y actualización.
            migrationBuilder.Sql(
                @"
                    PRAGMA foreign_keys = 0;
                    
                    CREATE TABLE PrizeSelectionSteps_Temp AS SELECT * FROM PrizeSelectionSteps;

                    DROP TABLE PrizeSelectionSteps;

                    CREATE TABLE 'PrizeSelectionSteps' (
                        'PrizeId' INTEGER NOT NULL,
                        'EntrantId' INTEGER NOT NULL,
                        'DrawEntryId' INTEGER NOT NULL,
                        'RegisteredOn' TEXT NOT NULL,
                        'PrizeSelectionStepType' INTEGER NOT NULL,
                        CONSTRAINT 'PK_PrizeSelectionSteps' PRIMARY KEY ('PrizeId', 'EntrantId', 'DrawEntryId'),
                        CONSTRAINT 'FK_PrizeSelectionSteps_Entrants_EntrantId' FOREIGN KEY ('EntrantId') REFERENCES 'Entrants' ('Id') ON DELETE CASCADE,
                        CONSTRAINT 'FK_PrizeSelectionSteps_Prizes_PrizeId' FOREIGN KEY('PrizeId') REFERENCES 'Prizes' ('Id') ON DELETE CASCADE
                        CONSTRAINT 'FK_PrizeSelectionSteps_DrawEntries_DrawEntryId' FOREIGN KEY('DrawEntryId') REFERENCES 'DrawEntries' ('Id') ON DELETE CASCADE
                    );

                    CREATE INDEX IF NOT EXISTS 'IX_PrizeSelectionSteps_PrizeId' ON 'PrizeSelectionSteps' ('PrizeId');
                    CREATE INDEX IF NOT EXISTS 'IX_PrizeSelectionSteps_EntrantId' ON 'PrizeSelectionSteps' ('EntrantId');
                    CREATE INDEX IF NOT EXISTS 'IX_PrizeSelectionSteps_DrawEntryId' ON 'PrizeSelectionSteps' ('DrawEntryId');

                    INSERT INTO PrizeSelectionSteps
                    SELECT 
                        PrizeId
                        , EntrantId
                        , 0
                        , RegisteredOn
                        , PrizeSelectionStepType
                    FROM PrizeSelectionSteps_Temp;
                    
                    DROP TABLE PrizeSelectionSteps_Temp;

                    PRAGMA foreign_keys = 1;
                "
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // La eliminación de claves primarias y foráneas no son soportadas por Sqlite, por lo que hay que hacer una actualización manual.
            // Se elimina restricción que puedan ejercer las FK en caso de eliminación o actualización.
            // Se crea una tabla temporal con los datos de la tabla PrizeSelectionSteps.
            // Se elimina la tabla con el schema actual.
            // Se crea una nueva tabla omitiendo la creación del campo y se restituye la antigua PK.
            // Se crean los indices para los campos que son parte del PK y que ademas son FK.
            // Se copian los datos que existian en la tabla.
            // Se elimina la tabla temporal.
            // Se restituye la restricción de FK en caso de eliminación y actualización.
            migrationBuilder.Sql(
                @"
                    PRAGMA foreign_keys = 0;
                    
                    CREATE TABLE PrizeSelectionSteps_Temp AS SELECT * FROM PrizeSelectionSteps;

                    DROP TABLE PrizeSelectionSteps;

                    CREATE TABLE 'PrizeSelectionSteps' (
                        'PrizeId' INTEGER NOT NULL,
                        'EntrantId' INTEGER NOT NULL,
                        'RegisteredOn' TEXT NOT NULL,
                        'PrizeSelectionStepType' INTEGER NOT NULL,
                        CONSTRAINT 'PK_PrizeSelectionSteps' PRIMARY KEY ('PrizeId', 'EntrantId'),
                        CONSTRAINT 'FK_PrizeSelectionSteps_Entrants_EntrantId' FOREIGN KEY ('EntrantId') REFERENCES 'Entrants' ('Id') ON DELETE CASCADE,
                        CONSTRAINT 'FK_PrizeSelectionSteps_Prizes_PrizeId' FOREIGN KEY('PrizeId') REFERENCES 'Prizes'('Id') ON DELETE CASCADE
                    );

                    INSERT INTO PrizeSelectionSteps
                    SELECT 
                        PrizeId
                        , EntrantId
                        , RegisteredOn
                        , PrizeSelectionStepType
                    FROM PrizeSelectionSteps_Temp;

                    CREATE INDEX IF NOT EXISTS 'IX_PrizeSelectionSteps_PrizeId' ON 'PrizeSelectionSteps' ('PrizeId');
                    CREATE INDEX IF NOT EXISTS 'IX_PrizeSelectionSteps_EntrantId' ON 'PrizeSelectionSteps' ('EntrantId');
                    
                    DROP TABLE PrizeSelectionSteps_Temp;

                    PRAGMA foreign_keys = 1;
                "
            );
        }
    }
}
