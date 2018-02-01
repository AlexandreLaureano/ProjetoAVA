CREATE TRIGGER PONTOS
ON pontuacoes
for INSERT
AS
BEGIN
    DECLARE
    @PONTO int,
	@ALUNO int

    SELECT @PONTO = Pontos, @ALUNO = AlunoId FROM INSERTED

    UPDATE Users SET PontoGeral = PontoGeral + @PONTO
    WHERE Id = @ALUNO
END
GO