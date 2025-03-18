# **Rick and Morty API - Challenge**

Este proyecto expone una API REST que interactúa con la API externa de **Rick and Morty** para obtener personajes y analizar las especies más comunes.

## **Arquitectura Implementada**

La arquitectura del proyecto sigue un enfoque **limpio** con cinco capas principales:

1. **Capa de Presentación (`ChallengeRickAndMortyApi`)**:
   - Esta capa es responsable de exponer la **API REST**. Contiene el controlador que interactúa con los servicios de la aplicación y exponen los endpoints. 
   - Ejemplo: El controlador `CharactersController` maneja las peticiones a los endpoints `/api/characters` y `/api/characters/top-species`.

2. **Capa de Aplicación (`ChallengeRickAndMortyApi.Application`)**:
   - Aquí se encuentra la **lógica de negocio** de la aplicación, como lo son DTOs, con los que mostramos los datos necesarios que se piden en el requerimiento.
   - Ejemplo: La clase `TopSpecies` solo se tiene las propiedades que se necesitan mostrar en el endpoint.
   - Esta capa también define interfaces como `IRickAndMortyApiService` y `IMemoryCacheService`.

3. **Capa de Dominio (`ChallengeRickAndMortyApi.Domain`)**:
   - Esta capa está compuesta por las **entidades** de la aplicación.
   - Ejemplo: La entidad `Personaje` representa a un personaje de Rick and Morty con sus propiedades, como nombre, especie, etc.

4. **Capa de Infraestructura (`ChallengeRickAndMortyApi.Infrastructure`)**:
   - En esta capa se gestionan las dependencias tecnológicas, como la implementación de servicios externos (API de Rick and Morty), el acceso a la caché y la configuración de logging.
   - Ejemplo: `RickAndMortyApiService` implementa la lógica de acceso a la API externa y `MemoryCacheService` implementa el acceso a caché.

5. **Capa de Pruebas (`ChallengeRickAndMortyApi.Test`)**:
   - Esta capa contiene las pruebas unitarias de la aplicación.
   - Se utilizan **xUnit** y **Moq** para simular las dependencias.

## **Decisiones Técnicas Clave**

- **Caché en Memoria**:  
  Se utiliza `IMemoryCacheService` para almacenar los resultados de las solicitudes de personajes y evitar múltiples consultas a la API externa.

- **Algoritmo de Especies Más Comunes**:  
  El algoritmo que calcula las **top 5 especies más comunes** agrupa los personajes por especie y cuenta la cantidad de veces que aparece cada especie, luego ordena y retorna las 5 más comunes.   

## **Cómo Ejecutar el Proyecto Localmente**

### 1. **Requisitos Previos**
   - Tener instalado **.NET 8** en tu máquina. (si lo vas a ejecutar desde el visual studio) 
   - Tener **Docker** instalado para la ejecución en contenedores.

### 2. **Clonar el Repositorio**

   ```bash
   git clone https://github.com/ocdamian/ChallengeRickAndMortyApi.git
   cd ChallengeRickAndMortyApi
   ```

### 3. **Configuración y Ejecución**

   Ejecuta el siguiente comando para crear la imagen y el contenedor en tu docker descktop:

   ```bash
   docker-compose up --build -d
   ```

   Esto iniciará la API en el puerto por defecto: `https://localhost:5000`.

### 5. **Probar la API**
   En el navegador accede al **Swagger** `http://localhost:5000/swagger/index.html`. 
   Usa una herramienta como **Postman** para realizar peticiones a los siguientes endpoints:

   - `GET http://localhost:5000/api/characters`: Obtiene la lista de personajes.
   - `GET http://localhost:5000/api/characters/top-species`: Obtiene las **top 5 especies más comunes**.

## **Algoritmo Implementado: Especies Más Comunes**

El algoritmo para calcular las **especies más comunes** sigue estos pasos:

1. Se recorre la lista de personajes, y se cuenta cuántos personajes hay por cada especie utilizando un **diccionario**.
2. Si la especie ya existe en el diccionario, simplemente se incrementa el contador; si no existe, se añade la especie con un valor inicial de 1.
3. Luego, se crea una lista de objetos `TopSpecies` a partir del diccionario, donde cada entrada contiene el nombre de la especie y su cantidad.
4. Se ordena de mayor a menor.
5. Se Obtienen los primeros 5 (el requerimiento lo indica)
6. Finalmente, se devuelve la lista de las 5 especies ordenadas por su cantidad.

### **Código del Algoritmo**:

```csharp
        public async Task<List<TopSpecies>> GetTopSpeciesAsync()
        {
            var characters = await GetCharactersAsync();

            Dictionary<string, int> speciesCount = new Dictionary<string, int>();

            foreach (var character in characters)
            {
                string species = character.Especie;
                if (speciesCount.ContainsKey(species))
                    speciesCount[species]++;
                else
                    speciesCount[species] = 1;
            }

            List<TopSpecies> topSpecies = new List<TopSpecies>();
            foreach (var species in speciesCount)
            {
                topSpecies.Add(new TopSpecies { Especie = species.Key, Cantidad = species.Value });
            }

            topSpecies.Sort((a, b) => b.Cantidad.CompareTo(a.Cantidad));
            
            List<TopSpecies> firstFive = new List<TopSpecies>();
            int limit = Math.Min(5, topSpecies.Count);
            for (int i = 0; i < limit; i++)
            {
                firstFive.Add(topSpecies[i]);
            }

            return firstFive;
        }
```

Este algoritmo se puede resolver utilizando LINQ de la siguiente forma.

```csharp
        public async Task<List<TopSpecies>> GetTopSpeciesAsync()
        {
            var characters = await GetCharactersAsync();

            return characters
                .GroupBy(s => s.Especie)
                .Select(x => new TopSpecies { Especie = x.Key, Cantidad = x.Count() })
                .OrderByDescending(x => x.Cantidad)
                .Take(5)
                .ToList();
        }
```


