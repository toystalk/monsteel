# Automação de Build

Este diretório contém scripts para realizar o build automático do projeto.

## Instalação

### Preparação da Unity

* Instale os SDKs de iPhone e Android (os que se aplicarem)
* Registre esses SDKs na Unity

### Preparação do sistema de Automação

* Copie o diretório AutoBuild para um local conveniente
* Abra o arquivo auto_build.py e edite as configurações relevantes
* Instale o Python 2.7.6 para Windows
* Edite o arquivo "run_build.bat" para refletir o local do diretório
* Instale o Git for Windows e certifique-se que o comando "git" esteja no seu PATH
* Abra o "Git Bash" e gere uma chave SSH através do comando ssh-keygen
* Coloque essa chave como uma Deploy Key do repositório no GitHub