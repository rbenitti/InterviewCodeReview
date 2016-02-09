Errores 
	En el método LogMessage, se repite el nombre de los parametros string message y bool message 
	En el método LogMessage, se utiliza la variable l sin que haya sido instanciada necesariamente (su instanciacion depende de la existencia del archivo de log).
	Se utiliza Trim sin chequear que la variable message este instanciada (puede ser una referencia nula)
	El comando SQL es propenso a SQL Injection, dado que se forma concatenando texto con la variable message. 
	
Estilo
	Se declaran variables que no se utilizan (_initialized), o se asignan pero no se utilizan (_logToFile, _logToConsole, LogToDatabase).
	El método LogMessage, la declaración de variables locales no se realiza al comienzo del scope.	
	LogToDatabase no cumple con el estilo de nombre deducible para los miembros privados (comienzan con underscore y minúscula).
	Se repite el código de armado del nombre del archivo de log, lo que da lugar a inconsistencias si alguna de las declaraciones se modifica pero la otra no.
	Se levantan excepciones genericas.
	Los nombres de las variables no son descriptivos.
	Los namespace System.Configuration y System.Data.SqlClient generan ruido (utilizar using).
	Se dejan sentencias using innecesarias.
		
Diseño
	No se cumple con el requerimiento de poder seleccionar a que destino se logueará (se intenta configurar seteando variables, pero luego éstas no se utilizan)
	Definir estáticamente los tipos de mensajes y los destinos de logueo resulta en código difícil de extender con nuevos tipos de mensaje y/o destinos de logueo
	Se requiere instanciar un objeto para setear valores estáticos o modificarlos, lo que es poco claro (el objeto se instancia pero no se utiliza).
	La clase JobLogger tiene demasiadas responsabilidades, lo que la vuelve compleja, dificil de modificar y dificil de testear. Actualmente, la clase se encarga de:
			La logica para decidir que tipo de logueo utilizar
			La logica para formatear los mensajes de acuerdo al tipo y destino
			La logica para impactar los mensajes en los distintas destinos
	El diseño es difícil de testear, pues se necesitan que muchas condiciones se cumplan para que la clase pueda utilizarse (las AppSettings deben estar instanciadas, debe existir una conexion a base de datos, la connectionString debe estar definida)
	Logueo en archivo: En lugar de realizar un Append al final del archivo, se intentar cargarlo completamente en la variable l y volver a escribirlo entero. Esto podria generar problemas como, por ejemplo, que se pisen distintos logueos.
	La configuración de logueo (nombres de archivos, colores, codigos asignados a cada tipo de mensaje) esta incrustada en la clase, por lo que habría que modificar su codigo fuente para poder modificarla.
	
Cambios:
	Crear clases con responsabilidades limitadas: más pequeñas y fáciles de testear e implementar.
	Programar contra interfaces, para reducir el acoplamiento de las distintas clases, y simplificar el agregado de nuevos loggers y tipos de mensajes.
	Hacer de la clase un singleton, de manera de la configuracion se logueo sea consistente a lo largo de toda la aplicación.
	Utilizar el patron Strategy para definir los distintas formas de logueo. Utilizar una colección de estrategias.
	Utilizar injección de dependencias, para lograr flexibilidad y facilidad de testeo.
	Abstraer la lógica de la base de datos y apertura de archivo (facilitar el testeo y reducir el acoplamiento). Utilizar Facade o Factory.
	Parametrizar la sentencia SQL, a fin de evitar la injeccion de SQL malicioso.
	Proporcionar formas de configurar los loggers sin modificar su codigo interno.