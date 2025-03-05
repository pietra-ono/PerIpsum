# PerIpsum
<br />

## Descrição

O **PerIpsum** é uma plataforma inovadora que conecta estudantes a oportunidades acadêmicas internacionais, como bolsas de estudo, intercâmbios e programas de pesquisa. Utilizando curadoria automatizada e manual, garantimos que todas as oportunidades sejam confiáveis e relevantes. A plataforma permite que os estudantes explorem, filtrem, favoritem e acompanhem prazos importantes em um calendário personalizado ou pelo feed de atualizações.

Além disso, instituições parceiras podem publicar suas oportunidades diretamente, promovendo conexões eficientes e seguras em um ambiente intuitivo e fácil de usar.

## Dependências

- **SQL Server**: Utilizado para armazenar dados da plataforma.
- **Visual Studio**: Ambiente de desenvolvimento para trabalhar com o projeto.

## Passos para Executar o Projeto

1. **Abrir o Projeto no Visual Studio**:
   - Abra o arquivo do projeto **PerIpsum** no Visual Studio.

2. **Configuração da Conexão com o Banco de Dados**:
   - Acesse o arquivo **appsettings.json** pelo Gerenciador de Soluções.
   - Encontre a seção **connectionString** e altere os seguintes parâmetros:
     - **data source**: Substitua pelo nome do seu servidor SQL.
     - **Database**: Substitua pelo nome do banco de dados que você criará.
   
   3. **Criar o Banco de Dados**:
      - Abra o SQL Server e execute a seguinte query para criar o banco de dados:
        ```sql
        CREATE DATABASE 'nome_do_database';
        ```

3. **Atualizar o Banco de Dados**:
   - No Visual Studio, abra o **Console do Gerenciador de Pacotes**:
     - Vá em **Ferramentas > Gerenciador de Pacotes NuGet > Console do Gerenciador de Pacotes**.
   - Execute o comando para aplicar as migrações e atualizar o banco de dados:
     ```bash
     update-database
     ```

4. **Instalar Dependências** (se necessário):
   - Caso haja dependências adicionais, elas podem ser instaladas por meio do **NuGet Package Manager** do Visual Studio ou via **Package Manager Console**.

5. **Executar o Projeto**:
   - Após configurar o banco de dados, as dependências e as migrações, execute o projeto no Visual Studio. O sistema estará pronto para ser utilizado.

## Como Contribuir

1. **Fork** este repositório para a sua conta do GitHub.
2. Crie uma **branch** para a funcionalidade ou correção que você deseja implementar:
   ```bash
   git checkout -b nome-da-sua-branch
