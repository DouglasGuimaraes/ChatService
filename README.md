# ChatService

## Descrição

Projeto de chat (cliente x servidor) utilizando o protocolo TCP para as conexões.

### Camadas

- **ChatService:** camada responsável pela aplicação Console no lado do Cliente.
- **ChatService.Infra:** camada responsável por serviços fora do domínio, como o que foi utilizado: recuperação do IP local.
- **ChatService.Models:** camada responsável pelos modelos do projeto incluindo seus contratos (Interfaces).
- **ChatService.Server:** camada responsável pela aplicação Console no lado do Servidor.
- **ChatService.Services:** "coração" do projeto, onde todos os acessos das aplicações cliente x servidor passam por ela para acessar suas funcionalidades principais e onde cada módulo do sistema contém seu prório serviço. 
Também contém os contratos (Interfaces) dos respectivos serviços.
- **ChatService.Tests:** camada que realiza os testes unitários utilizando XUnit.

