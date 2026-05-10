# language: pt
Funcionalidade: Sensores IoT
  Como operador da EcoCity
  Quero registrar leituras de sensores
  Para que alertas sejam gerados conforme limites

  Cenário: Temperatura alta gera Alerta de Calor
    Dado que envio uma leitura do sensor "temperatura" no local "Praca" com chave "celsius" valor 36
    Quando a requisição POST é enviada para "/api/SensoresIot"
    Então o código de resposta deve ser 201
    E o campo "alerta" da resposta deve ser "Alerta de Calor"
    E a resposta deve respeitar o schema "sensor.schema.json"

  Cenário: Nível de resíduo alto gera Esvaziamento Urgente
    Dado que envio uma leitura do sensor "residuo" no local "Lixeira 12" com chave "nivel" valor 95
    Quando a requisição POST é enviada para "/api/SensoresIot"
    Então o código de resposta deve ser 201
    E o campo "alerta" da resposta deve ser "Esvaziamento Urgente"

  Cenário: Temperatura amena retorna Normal
    Dado que envio uma leitura do sensor "temperatura" no local "Sala" com chave "celsius" valor 22
    Quando a requisição POST é enviada para "/api/SensoresIot"
    Então o código de resposta deve ser 201
    E o campo "alerta" da resposta deve ser "Normal"
