# Exemplos

## Exemplo 1

```json

POST /playwrights/_doc/1 

{ "firstname": "William", "lastname": "Shakespeare" }

```

```json

POST /playwrights/_doc/2

{ "firstname": "Samuel", "lastname": "Beckett", "year_of_birth": 1906 }

```

```json
GET /playwrights/_search

{ "query": { "match_all": {} } }

```

```json

GET /playwrights/_search
{
    "query": {
        "match": {
            "year_of_birth": 1906
        }
    }
}

```

## Exemplo 2

```json

PUT /produtos
{
 "settings": {
 "number_of_shards": 1,
 "number_of_replicas": 0
 }
}

```

```json

POST /produtos/_doc/1
{
  "nome": "Maçã Gala"
}
POST /produtos/_doc/2
{
  "nome": "Maçã Fuji"
}
POST /produtos/_doc/3
{
  "nome": "Banana Prata"
}


```

```json

GET /produtos/_search
{
  "query": {
    "fuzzy": {
      "nome": {
        "value": "Maca",
        "fuzziness": "AUTO"
      }
    }
  }
}

```

## Exemplo 3

```json

PUT /textos
{
  "settings": {
    "analysis": {
      "analyzer": {
        "default": {
          "tokenizer": "standard",
          "filter": ["lowercase", "english_stemmer"]
        }
      },
      "filter": {
        "english_stemmer": {
          "type": "stemmer",
          "language": "english"
        }
      }
    }
  }
}



```

```json


POST /textos/_doc/1
{
  "conteudo": "I am running"
}
POST /textos/_doc/2
{
  "conteudo": "He runs every day"
}
POST /textos/_doc/3
{
  "conteudo": "She is a runner"
}

```

```json

GET /textos/_search
{
  "query": {
    "match": {
      "conteudo": "run"
    }
  }
}

```

## Exemplo 4

```json

PUT /artigos
{
  "settings": {
    "number_of_shards": 1,
    "number_of_replicas": 0
  }
}

```json

POST /artigos/_doc/1
{
  "conteudo": "O café é uma bebida popular."
}
POST /artigos/_doc/2
{
  "conteudo": "Muitas pessoas gostam de café, mas algumas preferem chá."
}
POST /artigos/_doc/3
{
  "conteudo": "Café, chá e suco são opções de café da manhã."
}

```

```json

GET /artigos/_search
{
  "query": {
    "match": {
      "conteudo": "café"
    }
  }
}



```

## Exemplo 5

```json

PUT /blog_posts
{
  "settings": {
    "number_of_shards": 1,
    "number_of_replicas": 0
  }
}

```

```json

POST /blog_posts/_doc/1
{
  "conteudo": "Hoje aprendi sobre Elasticsearch e suas incríveis capacidades de busca."
}
POST /blog_posts/_doc/2
{
  "conteudo": "Este tutorial de Elasticsearch é muito útil para iniciantes."
}
POST /blog_posts/_doc/3
{
  "conteudo": "Configurar o Elasticsearch pode ser desafiador, mas é recompensador."
}

```

```json

GET /blog_posts/_search
{
  "query": {
    "match": {
      "conteudo": "Elasticsearch"
    }
  },
  "highlight": {
    "fields": {
      "conteudo": {}
    }
  }
}

```

## Exemplo 6

```json

PUT /comentarios
{
  "settings": {
    "number_of_shards": 1,
    "number_of_replicas": 0
  }
}

```

```json


POST /comentarios/_doc/1
{
  "texto": "Aprendendo sobre Elasticsearch, muito interessante!"
}
POST /comentarios/_doc/2
{
  "texto": "Este curso de Elasticsearch é excelente para iniciantes."
}
POST /comentarios/_doc/3
{
  "texto": "Dicas sobre Elasticsearch? Comece com a documentação oficial."
}



```

```JSON


GET /comentarios/_search
{
  "query": {
    "match": {
      "texto": "Elasticsearch"
    }
  }
}



```
