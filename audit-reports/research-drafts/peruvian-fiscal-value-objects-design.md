# **Auditoría y Diseño Arquitectónico Exhaustivo de Value Objects para el Ecosistema Fiscal Peruano en .NET 10**

## **1\. Marco Normativo y Fundamentos del Fiscal Domain Peruano**

El diseño de una arquitectura de software orientada al dominio fiscal peruano exige una alineación estricta con el marco legal, normativo y técnico establecido por la Superintendencia Nacional de Aduanas y de Administración Tributaria (SUNAT), el Código Tributario cuyo Texto Único Ordenado fue aprobado por el Decreto Supremo N.° 133-2013-EF, y las leyes sustantivas de los principales tributos del país1.  
El ecosistema de comprobantes de pago y libros electrónicos en el Perú opera bajo un modelo regulado mediante el Sistema de Emisión Electrónica (SEE) y el Sistema Integrado de Registros Electrónicos (SIRE)1. La semántica de los datos transaccionados porta efectos jurídicos e impositivos inmediatos, tales como la validez del crédito fiscal del Impuesto General a las Ventas (IGV) regulado en el TUO de la Ley del IGV (Decreto Supremo N.° 055-99-EF), la deducibilidad del gasto o costo para efectos del Impuesto a la Renta (Decreto Supremo N.° 179-2004-EF) y el cumplimiento de los sistemas de pago adelantado de obligaciones tributarias1.  
El marco normativo integra la legislación tributaria con especificaciones técnicas internacionales y nacionales. La emisión de Comprobantes de Pago Electrónicos (CPE) se rige por el estándar *Universal Business Language* (UBL 2.1) adaptado por la SUNAT mediante la Resolución de Superintendencia N.° 097-2012/SUNAT y sus modificatorias2. Por su parte, el traslado de bienes dentro del territorio nacional requiere la emisión de Guías de Remisión Electrónicas (GRE) bajo la Resolución de Superintendencia N.° 123-2022/SUNAT y la Resolución N.° 000108-2026/SUNAT1. Asimismo, la contabilidad tributaria obligatoria se realiza a través del SIRE para el Registro de Ventas e Ingresos Electrónico (RVIE) y el Registro de Compras Electrónico (RCE), normado por la Resolución de Superintendencia N.° 112-2021/SUNAT, la R.S. N.° 293-2024/SUNAT, la R.S. N.° 000392-2025/SUNAT y la R.S. N.° 000005-2026-SUNAT/7000001.

| Dominio Normativo | Base Legal / Regulación SUNAT | Documentos Técnicos Vigentes | Estabilidad y Dinámica Normativa |
| :---- | :---- | :---- | :---- |
| **Código Tributario** | TUO D.S. N.° 133-2013-EF | Normativa de Infracciones y Sanciones (Art. 175, 177\)1 | **Alta Estabilidad**: Cambios solo por Decreto Legislativo o Ley. |
| **Impuesto General a las Ventas (IGV)** | TUO D.S. N.° 055-99-EF y R.S. de Comprobantes de Pago (R.S. 007-99/SUNAT) | Catálogos 07 (Afectación), 14, 15, 16, 17 | **Estable con Variaciones de Tasa**: Tasa general (18%), regímenes especiales (10% MYPEs turísticas). |
| **Comprobantes Electrónicos (CPE)** | R.S. N.° 097-2012/SUNAT, R.S. N.° 188-2010/SUNAT y modificatorias | Anexos UBL 2.1, Reglas de Validación CPE actualizadas4 | **Dinámico**: Actualizaciones periódicas de Reglas de Validación y estructuras XML5. |
| **Sistema Detracciones (SPOT)** | Decreto Legislativo N.° 940 y R.S. N.° 183-2004/SUNAT | Catálogo 54 (Bienes y Servicios Sujetos a Detracción)8 | **Muy Dinámico**: Modificación frecuente de tasas y códigos de servicios gravados8. |
| **Guías de Remisión (GRE)** | R.S. N.° 123-2022/SUNAT y R.S. N.° 000108-2026/SUNAT1 | Estructuras UBL 2.1 GRE, Reglas de Validación GRE4 | **Moderadamente Dinámico**: Ajustes en Catálogo 62 (Bienes normalizados) y motivos de traslado3. |
| **SIRE (RVIE / RCE)** | R.S. N.° 112-2021/SUNAT, R.S. N.° 293-2024, R.S. N.° 000392-20251 | Estructuras e Instructivos de Archivos de Intercambio SIRE1 | **En Transición**: Cronogramas de obligatoriedad ajustados progresivamente hasta 20261. |

En la normativa peruana existen zonas de ambigüedad técnica y legal que el modelo de dominio debe absorber explícitamente. Entre ellas se encuentra la convivencia transitoria de catálogos de clasificación como el Catálogo N.° 25 de SUNAT (UNSPSC v14), cuya validación estricta a nivel de 8 dígitos ha sido aplazada por la SUNAT hasta el 1 de enero de 20279. Asimismo, la exigencia formal de bancarización establecida en la Ley N.° 28194 obliga al uso de Medios de Pago oficiales del Catálogo 59 para operaciones superiores a S/ 2,000 o US$ 500, condicionado a la deducibilidad fiscal independientemente del devengado contable10.

## **2\. Taxonomía y Categorización DDD del Fiscal Domain**

En Domain-Driven Design (DDD), un Value Object (VO) es una abstracción inmutable, definida exclusivamente por el valor de sus atributos, sin hilo de identidad conceptual en el tiempo. Debe poseer comportamiento encapsulado, autovalidación inmediata mediante invariantes y semántica de dominio expresiva.  
Para prevenir la *Primitive Obsession* sin incurrir en *Over-Modeling*, se establece una clasificación categórica rigurosa de los conceptos fiscales peruanos. No toda propiedad de un comprobante debe convertirse en un Value Object; la correcta discriminación arquitectónica asegura que el núcleo de dominio permanezca desacoplado de las infraestructuras de transporte o serialización XML/JSON de la SUNAT.

| Concepto Fiscal Candidato | Categoría DDD Correcta | Justificación Arquitectónica |
| :---- | :---- | :---- |
| **Número de RUC** | **Value Object** | Semántica propia, formato formal (11 dígitos), algoritmo de validación (Modulo 11\) e inmutabilidad. |
| **Contribuyente / Ficha RUC** | **Entity / Aggregate Root** | Posee identidad única (RUC), pero su estado cambia en el tiempo (Estado, Condición, Domicilio Fiscal). |
| **Documento de Identidad (Combinado)** | **Value Object** | Tupla inmutable (TipoDocumento, NumeroDocumento) con validación cruzada. |
| **Comprobante de Pago (Factura/Boleta)** | **Aggregate Root** | Entidad con ciclo de vida, estados (Emitido, Aceptado, Anulado), firma digital e identidad natural dada por su número correlativo. |
| **Serie de Comprobante** | **Value Object** | Formato regulado (4 caracteres alfanuméricos según el tipo de CPE) e inmutable. |
| **Correlativo de Comprobante** | **Value Object** | Secuencia numérica (1 a 8 dígitos), inmutable y con formateo específico de relleno con ceros a la izquierda. |
| **Identificador de Comprobante (CpeIdentifier)** | **Value Object** | Tupla inmutable (TipoComprobante, Serie, Correlativo) que actúa como clave natural de negocio. |
| **Monto Monetario (Money)** | **Value Object** | Par inmutable (Amount, Currency) con operaciones aritméticas que previenen mezclas no autorizadas de divisas. |
| **Tasa de Impuesto (TaxRate)** | **Value Object** | Porcentaje de cálculo (e.g., 0.18, 0.10, 0.04) con comportamiento matemático para extraer bases e impuestos. |
| **Mapeo UBL 2.1 (InvoiceType)** | **Integration Model / DTO** | Estructura XML orientada a la serialización del estándar OASIS UBL. Pertenece a la capa de integración2. |
| **Catálogo 01 SUNAT (Tipo Comprobante)** | **Catalog-Driven VO** | Encapsula la semántica fiscal, pero sus valores provienen de una fuente dinámica versionada. No debe ser un Enum C\# cerrado. |
| **Constancia de Depósito SPOT** | **Value Object / Entity** | En Facturación es un VO de referencia; en el Bounded Context de Detracciones es una Entidad de pago10. |
| **Servicio Web de Envío SUNAT** | **Domain Service** | Servicio encargado de coordinar el proceso de envío y recepción del CDR sin poseer estado persistente. |
| **Respuesta SUNAT (CDR Parseado)** | **DTO / Integration Model** | Estructura que representa la respuesta deserializada de la SUNAT o de la OSE (*Operador de Servicios Electrónicos*). |
| **Algoritmo Modulo 11** | **Policy / Specification** | Regla matemática pura encapsulada dentro del VO RUC o aislada como especificación de validación. |

## **3\. Identificación Tributaria y Contribuyente**

La identificación tributaria en el Perú distingue entre la identidad fiscal de los sujetos nacionales (personas naturales y jurídicas) y la de sujetos extranjeros no domiciliados.

### **Estructura y Validación del Registro Único de Contribuyentes (RUC)**

El Registro Único de Contribuyentes (RUC) es la piedra angular de la identificación fiscal peruana. Consiste en una cadena de 11 dígitos numéricos con una estructura algorítmica estricta:

> 1. **Prefijo de 2 dígitos**:  
   * 10: Natural Person con Negocio (asociado internamente al DNI de la persona).  
   * 15 / 17: Natural Person con otros documentos de identidad o sucesiones indivisas.  
   * 20: Legal Entity (Sociedades Anónimas, SRL, EIRL, Sociedades Civiles, Entidades Públicas).  
> 2. **Cuerpo de 8 dígitos**: Corresponde al DNI en las personas naturales (prefijo 10\) o a una secuencia correlativa asignada por la SUNAT para personas jurídicas.  
> 3. **Verification Check Digit (1 dígito)**: Calculado mediante el algoritmo Modulo 11 aplicado sobre los primeros 10 dígitos, utilizando los factores ponderadores \[5, 4, 3, 2, 7, 6, 5, 4, 3, 2\].

Para adquirentes en comprobantes de pago, la normativa reconoce múltiples documentos de identidad consolidados en el **Catálogo N.° 06**: DNI (8 dígitos numéricos), Carné de Extranjería (hasta 12 caracteres alfanuméricos), Pasaporte (hasta 12 caracteres alfanuméricos) y Cédula Diplomática de Identidad.  
El modelo de dominio evita las jerarquías de herencia orientadas a objetos para los documentos de identidad por su rigidez. En su lugar, define el Value Object Ruc como un tipo especializado de alto rendimiento con validaciones nativas de Modulo 11, mientras que los demás documentos se modelan mediante el Value Object compuesto IdentityDocument, el cual relaciona el código del Catálogo 06 con el número respectivo.

### **Modelado del Contribuyente: Entity vs. Value Object**

Un contribuyente en el dominio fiscal peruano **no es un Value Object**. Un contribuyente posee un ciclo de vida, un estado cambiante en los padrones de la SUNAT y mutabilidad en sus atributos tributarios. Por lo tanto, el Taxpayer se modela como un **Aggregate Root / Entidad**, cuya identidad está determinada por su Value Object Ruc. Sus atributos descriptivos se componen de Value Objects especializados:

* **Identity**: Ruc (Value Object).  
* **RazonSocial**: Value Object de texto normalizado, filtrando caracteres de control no permitidos en el estándar UBL XML.  
* **NombreComercial**: Value Object opcional.  
* **TaxpayerStatus**: Value Object o Tipo de Catálogo (e.g., ACTIVO, BAJA DE OFICIO, BAJA DEFINITIVA).  
* **TaxpayerCondition**: Value Object o Tipo de Catálogo (e.g., HABIDO, NO HABIDO, NO HALLADO). La condición de "NO HABIDO" invalida el ejercicio del crédito fiscal del IGV para los adquirentes según el Artículo 18 de la Ley del IGV.  
* **FiscalAddress**: Value Object Address con estructura de Ubigeo y dirección física.  
* **TaxRegime**: Value Object que representa el régimen tributario (e.g., General, MYPE Tributario, Especial \- RER, Popular).

## **4\. Comprobantes de Pago y Comprobantes Electrónicos (CPE / SEE)**

### **Estructura de Identificación del Comprobante de Pago**

Todo comprobante de pago emitido bajo la normativa del Reglamento de Comprobantes de Pago (R.S. N.° 007-99/SUNAT) y las regulaciones del SEE posee una identificación tripartita única e inmutable:

> 1. **Tipo de Documento (Catálogo 01\)**: Factura (01), Boleta de Venta (03), Liquidación de Compra (04), Credit Note (07), Debit Note (08), Dispatch Guide Remitente (09), Dispatch Guide Transportista (31), Comprobante de Retención (20), Comprobante de Percepción (40).  
> 2. **Serie del Comprobante (CpeSeries)**:  
   * Para emisión electrónica (SEE-SOL, SEE-Del Contribuyente, SEE-OSE): 4 caracteres.  
   * Facturas: Inicia obligatoriamente con F seguido de 3 caracteres alfanuméricos (e.g., F001, FA01).  
   * Boletas: Inicia obligatoriamente con B seguido de 3 caracteres alfanuméricos (e.g., B001, BB01).  
   * Guías de Remisión: Inicia con EG01, T001 o T seguido de 3 números según el sistema.  
   * Contingencia física: 4 dígitos numéricos (e.g., 0001).  
> 3. **Número Correlativo (CpeCorrelative)**: Entero positivo entre 1 y 99,999,999, representado fiscalmente con relleno de ceros a la izquierda hasta 8 posiciones (00000001).

El Value Object CpeIdentifier encapsula esta tupla inmutable (DocumentType, CpeSeries, CpeCorrelative) ofreciendo una representación de cadena formateada única de negocio (e.g., 01-F001-00000123).

### **Desacoplamiento de las Estructuras UBL 2.1 y XML**

Un error recurrente en la arquitectura de software fiscal es contaminar el modelo de dominio con las estructuras del estándar UBL 2.1 (*Universal Business Language*) o con las clases C\# generadas automáticamente a partir de los esquemas XSD de la SUNAT4.  
Conceptos como UBLVersionID, CustomizationID, ProfileID, cac:AccountingSupplierParty o los hashes digest de firmas XML **no pertenecen al Núcleo del Fiscal Domain**, sino a la **Capa de Integración y Serialización**.

| Concepto Técnico XML / UBL | Clasificación Arquitectónica | Explicación y Dominio Correcto |
| :---- | :---- | :---- |
| UBLVersionID (2.1) | **Technical Primitive / Configuration** | Parámetro de serialización del canal XML. Invariante técnico de transporte4. |
| CustomizationID (2.0) | **Technical Primitive / Configuration** | Identificador del esquema de validación SUNAT. |
| IssueDate | **Domain Concept \-\> DateOnly / FiscalDate** | Fecha de emisión. Determina el periodo tributario del IGV y el momento de devengo. |
| IssueTime | **Technical Primitive / TimeOnly** | Requisito de timbrado XML; carece de impacto en el cálculo de impuestos. |
| UUID / DigestValue | **Infrastructure Security Artifact** | Hash SHA-256 de la firma digital XML. Perteneciente al Bounded Context de Criptografía. |
| ResponseCode (CDR) | **Integration Model / DTO** | Código de respuesta retornado por la SUNAT u OSE (e.g., 0 para aceptado, 2000-3999 para rechazos). |

## **5\. Dominio Monetario y Tributación (IGV, Renta, Retenciones, Percepciones, Detracciones)**

### **El Value Object Monetario (Money) y Tipos de Cambio**

El manejo del dinero en el sistema fiscal peruano exige la imposibilidad matemática de realizar operaciones aritméticas entre importes expresados en monedas distintas sin una conversión explícita soportada por una tasa de cambio oficial. La SUNAT exige que los comprobantes emitidos en moneda extranjera (e.g., USD) muestren el tipo de cambio oficial publicado por la Superintendencia de Banca, Seguros y AFP (SBS) a la fecha de emisión.  
El Value Object Money almacena el valor decimal con una precisión de al menos 4 decimales para cálculos intermedios de precios unitarios y 2 decimales para el redondeo fiscal final. El redondeo exigido por las reglas de validación de SUNAT sigue el estándar de redondeo bancario (*Half-Even* o MidpointRounding.ToEven).  
El Value Object ExchangeRate encapsula la tasa de conversión tributaria, asociando la moneda de origen, la moneda de destino, el valor decimal de la tasa (precisión SBS de 3 a 4 decimales) y la fecha de vigencia de la cotización.

### **Arquitectura del IGV e Impuesto a la Renta**

El Impuesto General a las Ventas (IGV) grava la venta de bienes muebles, la prestación de servicios, los contratos de construcción y las importaciones. El modelo de dominio no se limita a un porcentaje flotante; clasifica la afectación según el **Catálogo N.° 07**:

> 1. **Operaciones Gravadas**: Sujetas a la tasa general (18%, compuesta por 16% IGV \+ 2% Impuesto de Promoción Municipal) o tasas reducidas (e.g., 10% para regímenes MYPE turísticos).  
> 2. **Operaciones Exoneradas**: Incluidas en los Apéndices I y II de la Ley del IGV (e.g., productos agrícolas primarios). No generan IGV, pero forman parte de la base imponible exonerada.  
> 3. **Operaciones Inafectas**: Operaciones fuera del ámbito de aplicación del impuesto (e.g., indemnizaciones, transferencias fiduciarias).  
> 4. **Exportation de Bienes y Servicios**: Operaciones gravadas con la tasa del 0%, con derecho a la devolución del Saldo a Favor Materia del Beneficio (SFMB).  
> 5. **Operaciones Gratuitas**: Transferencias a título gratuito donde se calcula el "Valor Referencial" del impuesto para fines informativos tributarios, afectando el retiro de bienes.

El Impuesto a la Renta pertenece conceptualmente a la contabilidad tributaria y al cierre fiscal del ejercicio. El dominio de facturación únicamente captura las retenciones de Renta de Segunda, Cuarta (8% para Recibos por Honorarios) o Quinta Categoría cuando corresponde la deducción en el comprobante.

### **Sistemas de Pago Adelantado del IGV: SPOT (Detracciones), Retenciones y Percepciones**

Los mecanismos administrativos de recaudación imponen reglas de negocio estrictas sobre los comprobantes y los pagos:

> 1. **Sistema de Pago de Obligaciones Tributarias \- SPOT (Detracciones)**: Aplica cuando el comprobante supera el monto mínimo (S/ 700.00 para servicios en general) y la operación está catalogada en el **Catálogo N.° 54**8.  
   * DetractionAccount: Value Object que representa la cuenta corriente en el Banco de la Nación (cadena de 11 dígitos con formato 00-XXX-XXXXXX).  
   * DetractionServiceCode: Encapsulado como Catalog-Driven VO (Catálogo 54\)8.  
   * DetractionInformation: Value Object compuesto por el código de servicio, la tasa de detracción, el monto calculado en soles (PEN) y la cuenta de destino8.  
> 2. **Régimen de Retenciones del IGV**: Aplica cuando el adquirente es designado "Agente de Retención". Retiene el 3% (o la tasa vigente) del importe total de la operación al momento del pago y emite un Comprobante de Retención Electrónico (CRE \- Tipo 20). Se modela reutilizando el VO TaxRate y calculando el importe mediante Money.  
> 3. **Régimen de Percepciones del IGV**: Aplica cuando el vendedor es designado "Agente de Percepción" (e.g., venta interna de bienes gravados, combustible, importación definitiva). El vendedor cobra un porcentaje adicional sobre el precio de venta (e.g., 2%, 1%, 0.5%) y emite un Comprobante de Percepción Electrónico (CPE \- Tipo 40).

## **6\. Guías de Remisión Electrónicas (GRE)**

La Dispatch Guide Electrónica es el documento tributario que sustenta el traslado físico de bienes dentro del territorio nacional, regulado por la R.S. N.° 123-2022/SUNAT y la R.S. N.° 000108-2026/SUNAT1.  
La normativa distingue dos tipos de comprobantes independientes pero vinculados:

> 1. **GRE Remitente (Tipo 09\)**: Emitida por el propietario, poseedor o remitente de los bienes.  
> 2. **GRE Transportista (Tipo 31\)**: Emitida por la empresa de transporte público que presta el servicio al remitente2.

El dominio de traslado requiere los siguientes Value Objects especializados:

* **VehicleLicensePlate**: Placa de rodaje representada por una cadena de 6 caracteres alfanuméricos (e.g., ABC-123 o A1B-234), validando el formato oficial de la Tarjeta de Identificación Vehicular expedida por el Ministerio de Transportes y Comunicaciones (MTC).  
* **DriverInformation**: Value Object compuesto por el IdentityDocument del conductor y el número de Licencia de Conducir (cadena de 9 dígitos numéricos o alfanuméricos).  
* **GrossWeight**: Value Object compuesto por la cantidad decimal del peso total de la carga y la unidad de medida oficial del **Catálogo N.° 03** (e.g., KGM para Kilogramos, TNE para Toneladas métricas).  
* **TransferReasonCode**: Catalog-Driven VO respaldado por el **Catálogo N.° 20** (e.g., Venta, Compra, Traslado entre establecimientos, Importación, Exportation).  
* **TransportModeCode**: Catalog-Driven VO respaldado por el **Catálogo N.° 18** (Transporte Público vs. Transporte Privado).

## **7\. Geografía, Direcciones y Catálogos SUNAT**

### **Estructura Geográfica: El Ubigeo Peruano**

El código de Ubicación Geográfica (UBIGEO) administrado por el INEI y utilizado por la SUNAT consiste en una cadena de 6 dígitos numéricos dividida jerárquicamente:

* **Dígitos 1-2**: Código del Departamento (e.g., 15 para Lima).  
* **Dígitos 3-4**: Código de la Provincia (e.g., 01 para Lima).  
* **Dígitos 5-6**: Código del Distrito (e.g., 01 para Cercado de Lima, 14 para Miraflores).

El Value Object UbigeoCode encapsula estos 6 dígitos, ofreciendo métodos de consulta inmutables para determinar el departamento, provincia y distrito, garantizando la existencia del código dentro del padrón oficial.

### **El Value Object Direccional Unificado (Address)**

Para evitar la duplicación innecesaria de clases como FiscalAddress, ShippingAddress o DeliveryAddress, el sistema adopta un **único Value Object de dominio denominado Address**. El rol de negocio (fiscal, comercial, punto de partida, punto de llegada) es asignado por el nombre de la propiedad o la entidad que lo contiene.  
El Value Object Address contiene:

* Ubigeo: UbigeoCode (6 dígitos numéricos).  
* StreetName: Cadena normalizada en UTF-8 (máximo 100 caracteres).  
* ZoneOrUrbanization: Cadena opcional (e.g., "Urb. Los Cerezos").  
* AddressNumber: Cadena opcional (e.g., "123", "Mz A Lte 4").  
* BuildingInterior: Cadena opcional (e.g., "Dpto 302").  
* CountryCode: Código ISO 3166-1 Alpha-2 (e.g., PE, CL, US).

### **Gestión Arquitectónica de los Catálogos SUNAT**

La SUNAT administra decenas de catálogos paramétricos (Catálogos 01 al 59 y extensiones)8. Convertir cada catálogo de la SUNAT en un enum de C\# representa un error grave de diseño.  
Los catálogos de la SUNAT son **mutables por disposición administrativa**. La SUNAT modifica periódicamente las Reglas de Validación incorporando o eliminando códigos sin requerir cambios en el Código Tributario (e.g., la supresión de los códigos 038, 042 y 043 del Catálogo 54 de detracciones realizada en abril de 2025\)8.  
Si un catálogo se codifica como un enum cerrado en C\#:

> 1. Cada actualización de la SUNAT obliga a recompilar, probar y desplegar el código fuente del sistema.  
> 2. Los comprobantes históricos emitidos con códigos derogados fallarán al deserializarse si el enum fue alterado.

Por lo tanto, los catálogos de la SUNAT se modelan como **Catalog-Driven Value Objects**: tipos estructurales inmutables (readonly record struct) que encapsulan una clave de texto cuya validez se comprueba contra un servicio de catálogo dinámico versionado en memoria o persistido en base de datos.

## **8\. Libros Electrónicos, SIRE y Declaraciones Tributarias**

El Sistema Integrado de Registros Electrónicos (SIRE) reemplaza al Programa de Libros Electrónicos (PLE), obligando a los contribuyentes a llevar el Registro de Ventas e Ingresos Electrónico (RVIE) y el Registro de Compras Electrónico (RCE) mediante la aceptación, complementación o reemplazo de las propuestas generadas por la SUNAT1.  
De acuerdo con la R.S. N.° 293-2024/SUNAT, la R.S. N.° 000392-2025/SUNAT y la R.S. N.° 000005-2026-SUNAT/700000, la obligatoriedad del SIRE para los Principales Contribuyentes (PRICOS) fue postergada progresivamente para iniciar en enero o junio de 2026 según el nivel de ingresos netos del ejercicio 2024, otorgando plazos adicionales de subsanación sin sanciones administrativas1.  
El intercambio técnico con el SIRE (archivos de texto estructurados con separador pipe | o API REST) **pertenece a la Capa de Integración**. El dominio fiscal abstrae únicamente los conceptos de control:

* **Periodo Tributario (TaxPeriod)**: Value Object inmutable que representa la tupla (Año, Mes) en formato numérico YYYYMM (e.g., 202601 para enero de 2026).  
* **Código de Anotación del Registro (CAR)**: Cadena de 27 caracteres que identifica de forma unívoca el registro de un comprobante dentro del SIRE, uniendo el RUC del emisor, tipo de comprobante, serie y correlativo.

## **9\. Productos, Servicios, Pagos y Seguridad**

### **Product Code SUNAT y Unidades de Medida**

La SUNAT exige la clasificación estandarizada de productos y servicios mediante el **Catálogo N.° 25** (UNSPSC v14\_0801). La validación estricta de este código a nivel de 8 dígitos numéricos entrará en vigencia obligatoria el 1 de enero de 20279.

* **SunatProductCode**: Value Object de 8 dígitos numéricos validado contra la taxonomía de la UNSPSC9.  
* **UnitOfMeasureCode**: Catalog-Driven VO basado en el **Catálogo N.° 03** (e.g., NIU para Unidades, ZZ para Servicios, KGM para Kilogramos).

### **Dominio de Pagos y Bancarización**

La Ley N.° 28194 exige el uso de Medios de Pago autorizados para operaciones que superen los S/ 2,000 o US$ 50010. El **Catálogo N.° 59** enumera los medios de pago válidos (e.g., Depósito en cuenta 001, Transferencia de fondos 003, Tarjeta de crédito 006\)10.

* **PaymentTerm**: Value Object que representa la condición de pago del comprobante (Contado vs. Crédito). En comprobantes al crédito, encapsula el monto neto pendiente y la lista de cuotas.  
* **Installment**: Value Object que representa una cuota individual de pago (Número correlativo de cuota, Fecha de vencimiento DateOnly y Monto Money).

### **Firma Digital y Criptografía**

La firma digital XML (XAdES-BES) y los algoritmos hashing (SHA-256) **no pertenecen al dominio fiscal**. Son artefactos tecnológicos manejados exclusivamente por la Capa de Infraestructura Criptográfica. El dominio fiscal solo almacena, si requiere trazabilidad de auditoría, la representación en cadena del hash resultante como una propiedad técnica pasiva.

## **10\. Auditoría de Duplicidades y Racionalización del Dominio**

Para cumplir con el principio de diseñar el mínimo conjunto completo y no redundante de Value Objects, se ejecuta una auditoría de unificación conceptual sobre los candidatos analizados.

| Concepto Candidato A | Concepto Candidato B | Decisión Arquitectónica | Justificación Técnica y Términos Unificados |
| :---- | :---- | :---- | :---- |
| Ruc | TaxpayerId | **Especializar / Fusión** | En Perú, todo identificador tributario nacional es un Ruc. Para extranjeros se usa IdentityDocument. Se elimina TaxpayerId. |
| DocumentNumber | InvoiceNumber | **Reemplazar por CpeIdentifier** | "DocumentNumber" es ambiguo. CpeIdentifier consolida la tupla (TipoDocumento, Serie, Correlativo). |
| DocumentSeries | Series | **Unificar en CpeSeries** | Unifica el Value Object de serie aplicando las reglas de formato alfanumérico del SEE. |
| TaxRate | Percentage | **Especializar TaxRate** | Percentage es genérico. TaxRate contiene reglas tributarias (e.g., extracción de base imponible Base \= Total / (1 \+ Rate)). |
| TaxAmount | Amount | **Eliminar VO explícito** | El monto de un impuesto es una instancia simple del VO monetario universal Money. No se requiere una clase separada. |
| Money | MonetaryAmount | **Unificar en Money** | Redundancia absoluta. Se mantiene Money como el estándar universal. |
| TaxBase | TaxableAmount | **Eliminar VO explícito** | La base imponible es una magnitud de Money. Se modela como la propiedad TaxableBase: Money dentro de la afectación. |
| FiscalPeriod | TaxPeriod | **Unificar en TaxPeriod** | TaxPeriod encapsula la representación formal YYYYMM usada por SUNAT en SIRE, RVIE y RCE1. |
| Address | FiscalAddress | **Unificar en Address** | La estructura física de la dirección es única. El rol "Fiscal" se asigna por el nombre de la propiedad en el Taxpayer. |
| Ubigeo | DistrictCode | **Unificar en UbigeoCode** | El Ubigeo de 6 dígitos engloba Departamento, Provincia y Distrito de forma jerárquica. |
| Currency | CurrencyCode | **Unificar en CurrencyCode** | Representa la clave ISO 4217 de 3 letras de la moneda (Catálogo 02: PEN, USD). |
| Hash | Digest | **Eliminar del Núcleo Fiscal** | Artefactos pertenecientes al Bounded Context de Criptografía e Infraestructura XML. |
| WithholdingRate | TaxRate | **Reutilizar TaxRate** | La tasa de retención es un TaxRate validado en el rango específico (e.g., 3%). |
| DetractionRate | TaxRate | **Reutilizar TaxRate** | La tasa de detracción utiliza la estructura e interfaz de TaxRate. |
| DocumentId | CpeId | **Unificar en CpeIdentifier** | Evita la colisión semántica entre claves primarias de base de datos (Guid/Long) e identificadores de comprobantes. |

## **11\. Bounded Contexts, Context Map y API Boundary Matrix**

El ecosistema fiscal peruano se descompone en Bounded Contexts aislados con responsabilidades claras:

> 1. **Taxpayer Context (Padrón)**: Administración de contribuyentes, condicionales fiscales (Habido/No Habido), domicilios y regímenes tributarios.  
> 2. **Billing Context (Facturación)**: Creación comercial de comprobantes, cálculo de líneas, precios, descuentos y condiciones de pago.  
> 3. **CPE Context (Comprobantes Electrónicos)**: Aplicación de reglas de emisión electrónica del SEE, asignación de series y números correlativos.  
> 4. **SUNAT Integration Context**: Comunicación SOAP/REST, empaquetado ZIP, generación de XML UBL y recepción de CDR1.  
> 5. **Tax Compliance Context (Impuestos y Libros)**: Control de SIRE (RVIE/RCE)1, detracciones8, retenciones, percepciones y declaraciones Juntas.  
> 6. **Despatch Context (Guías de Remisión \- GRE)**: Control logístico de traslados, vehículos, conductores y puntos de llegada/partida3.

### **Matriz de Pertinencia de Value Objects por Contexto Delimitado**

| Value Object | Taxpayer | Billing | CPE | SUNAT Integration | Tax Compliance | Despatch (GRE) | Clasificación de Compartición |
| :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- |
| Ruc | **Propietario** | Consumidor | Consumidor | Consumidor | Consumidor | Consumidor | **Shared Kernel (Universal)** |
| IdentityDocument | Consumidor | **Propietario** | Consumidor | Consumidor | Consumidor | Consumidor | **Shared Kernel (Universal)** |
| CpeIdentifier | No usado | Consumidor | **Propietario** | Consumidor | Consumidor | Consumidor | **Shared Kernel Fiscal** |
| CpeSeries | No usado | Consumidor | **Propietario** | Consumidor | Consumidor | Consumidor | **Shared Kernel Fiscal** |
| Money | No usado | **Propietario** | Consumidor | Consumidor | Consumidor | No usado | **Shared Kernel Domain Primitives** |
| TaxRate | No usado | Consumidor | Consumidor | No usado | **Propietario** | No usado | **Shared Kernel Fiscal** |
| Address | Consumidor | Consumidor | Consumidor | No usado | No usado | **Propietario** | **Shared Kernel Domain Primitives** |
| UbigeoCode | Consumidor | No usado | Consumidor | No usado | No usado | **Propietario** | **Shared Kernel Domain Primitives** |
| DetractionAccount | No usado | Consumidor | Consumidor | No usado | **Propietario** | No usado | **Context-Specific (Tax Compliance)** |
| VehicleLicensePlate | No usado | No usado | No usado | No usado | No usado | **Propietario** | **Context-Specific (Despatch)** |
| TaxPeriod | No usado | No usado | No usado | Consumidor | **Propietario** | No usado | **Context-Specific (Tax Compliance)** |

### **API Boundary Matrix: Reglas de Exposición y Dependencias**

| Value Object | Visibilidad API | Paquete Propietario | Consumidores Permitidos | Consumidores Prohibidos | Exposición Permitida |
| :---- | :---- | :---- | :---- | :---- | :---- |
| Ruc | **Pública** | EricksonLopez.Peru | Todos los Contextos | Ninguno | Propiedades, JSON, Drivers BD |
| Money | **Pública** | EricksonLopez.DomainPrimitives | Todos los Contextos | Ninguno | Operaciones matemáticas, JSON |
| CpeIdentifier | **Pública** | EricksonLopez.Fiscal | Billing, CPE, Integration, Compliance | Padrón Base, Inventario Puro | Formateo fiscal de cadena |
| DetractionAccount | **Interna / Contextual** | EricksonLopez.Fiscal | Billing, Compliance, CPE | Despatch, Inventario, Seguridad | Solo como cadena de valor |
| VehicleLicensePlate | **Pública** | EricksonLopez.Peru | Despatch, CPE | Compliance, Contabilidad | Formato de placa MTC |

## **12\. Capa Anticorrupción (ACL) y Mapeos de Integración**

El núcleo de dominio no debe poseer ninguna dependencia con los esquemas XML UBL 2.1 de la SUNAT, payloads JSON del SIRE ni servicios WSDL SOAP1. La Capa Anticorrupción (ACL) ejecuta la transformación bidireccional entre el modelo de dominio y los contratos externos:

> 1. **Fiscal Domain \-\> UBL 2.1 Serializer**:  
   * El VO CpeIdentifier (e.g., Tipo 01, Serie F001, Correlativo 123\) se traduce a los elementos XML: \<cbc:InvoiceTypeCode listID="0101"\>01\</cbc:InvoiceTypeCode\> y \<cbc:ID\>F001-00000123\</cbc:ID\>.  
   * El VO Money se mapea a: \<cbc:PayableAmount currencyID="PEN"\>118.00\</cbc:PayableAmount\>.  
   * El VO Ruc se mapea a: \<cbc:CustomerAssignedACCOUNTID\>20123456789\</cbc:CustomerAssignedACCOUNTID\>.  
> 2. **Respuesta CDR SUNAT \-\> Fiscal Domain**:  
   * El archivo CDR (Constancia de Recepción) retorna un XML firmado que contiene el nodo \<cbc:ResponseCode\>0\</cbc:ResponseCode\>.  
   * La ACL deserializa dicho código de bajo nivel y lo mapea hacia un objeto de resultado del dominio: CpeProcessingResult.Accepted(CpeIdentifier, TicketNumber).

## **13\. Especificación de Contratos y API de Value Objects en .NET 10**

Los Value Objects se especifican formalmente utilizando las capacidades de **.NET 10**, apoyándose en readonly record struct para garantizar inmutabilidad y cero asignaciones de memoria en el Heap (*Zero-Allocations*), e implementando las interfaces ISpanParsable\<T\> e IUtf8SpanParsable\<T\>.

### **Contrato Conceptual del Value Object Ruc**

C\#  
namespace EricksonLopez.Peru;

public readonly record struct Ruc : ISpanParsable\<Ruc\>, IUtf8SpanParsable\<Ruc\>  
{  
    public string Value { get; }

    public static Result\<Ruc\> Create(string input);  
    public static bool TryCreate(ReadOnlySpan\<char\> input, out Ruc result);  
    public static Ruc Parse(ReadOnlySpan\<char\> s, IFormatProvider? provider);  
    public static bool TryParse(ReadOnlySpan\<char\> s, IFormatProvider? provider, out Ruc result);  
    public static Ruc Parse(ReadOnlySpan\<byte\> utf8Text, IFormatProvider? provider);  
    public static bool TryParse(ReadOnlySpan\<byte\> utf8Text, IFormatProvider? provider, out Ruc result);

    public bool IsPersonaJuridica { get; }  
    public bool IsPersonaNaturalConNegocio { get; }  
    public Result\<NationalIdentityCard\> ExtractDni();  
}

* **Invariantes del VO Ruc**:  
  1. Cadena de exactamente 11 dígitos numéricos ASCII ('0' a '9').  
  2. Prefijo inicial igual a 10, 15, 17 o 20\.  
  3. Undécimo dígito estrictamente igual al dígito verificador calculado mediante Modulo 11 con los pesos ponderados \[5, 4, 3, 2, 7, 6, 5, 4, 3, 2\].

### **Contrato Conceptual del Value Object Money**

C\#  
namespace EricksonLopez.DomainPrimitives;

public readonly record struct Money : IEquatable\<Money\>, IComparable\<Money\>  
{  
    public decimal Amount { get; }  
    public CurrencyCode Currency { get; }

    public static Money FromPen(decimal amount);  
    public static Money FromUsd(decimal amount);  
    public static Result\<Money\> Create(decimal amount, CurrencyCode currency);

    public Result\<Money\> Add(Money other);  
    public Result\<Money\> Subtract(Money other);  
    public Money MultiplyBy(decimal factor);  
    public Money RoundToFiscalTwoDecimals();  
}

* **Invariantes del VO Money**:  
  1. Currency debe ser un código ISO 4217 válido perteneciente al Catálogo 02 de la SUNAT.  
  2. Amount almacena internamente la precisión decimal original de hasta 4 posiciones.  
  3. La suma o resta entre instancias con Currency distinta retorna un error de dominio sin realizar conversiones implícitas.  
  4. RoundToFiscalTwoDecimals() aplica redondeo simétrico al par más cercano (MidpointRounding.ToEven).

## **14\. Integración con el Patrón Result y Códigos de Error Fiscales**

Para evitar el uso costoso de excepciones en flujos esperados de validación de dominio, la creación de Value Objects utiliza el tipo Result\<T\>.

### **Estructura de Códigos de Error Fiscales**

* FiscalError.Ruc.InvalidLength: El RUC no posee exactamente 11 dígitos.  
* FiscalError.Ruc.InvalidPrefix: El RUC no inicia con 10, 15, 17 o 20\.  
* FiscalError.Ruc.InvalidCheckDigit: El dígito verificador no coincide con el cálculo del Modulo 11\.  
* FiscalError.CpeSeries.InvalidFormat: La serie no cumple con la máscara alfanumérica requerida para el tipo de documento.  
* FiscalError.CpeCorrelative.OutOfRange: El correlativo es menor a 1 o superior a 99,999,999.  
* FiscalError.Money.CurrencyMismatch: Intento de operación aritmética entre divisas distintas sin conversión de cambio.  
* FiscalError.TaxPeriod.InvalidFormat: El periodo tributario no cumple con el patrón YYYYMM o corresponde a un mes inválido.

## **15\. Persistencia, Serialización y Desempeño (.NET 10 / Native AOT)**

### **Reglas de Asignación de Memoria (.NET 10\)**

> 1. **readonly record struct**: Utilizado para todos los Value Objects escalares de tamaño menor a 64 bytes (Ruc, CpeSeries, CpeCorrelative, CpeIdentifier, Money, CurrencyCode, UbigeoCode, TaxPeriod, VehicleLicensePlate).  
   * **Beneficio**: Asignación en el *Stack*, cero recolección de basura (*Zero GC allocations*), paso por referencia eficiente mediante la palabra clave in e igualdad por valor optimizada en tiempo de compilación.  
> 2. **readonly class**: Reservado para Value Objects compuestos que superan los 64 bytes o contienen múltiples cadenas de texto de longitud variable (e.g., Address). Evita el costo de copia excesiva de estructuras grandes en la pila de llamadas (*Stack*).

### **Estrategia de Persistencia y Serialización Compatible con Native AOT**

* **JSON (System.Text.Json)**: Implementación de JsonConverter\<T\> mediante *Source Generators* de C\# para Native AOT. Deserializa los Value Objects directamente desde cadenas o números simples sin usar reflexión.  
* **PostgreSQL / EF Core**: Mapeo mediante HasConversion() en Entity Framework Core, transformando el VO a su representación nativa en base de datos (varchar, numeric, char).  
* **Dapper**: Registro global de SqlMapper.TypeHandler\<T\> para materializar columnas de BD hacia Value Objects de forma directa.  
* **CPE XML / UBL**: Mapeo canalizado mediante XmlWriter y XmlReader con optimización de Spans para evitar asignaciones intermedias de cadenas4.

## **16\. Volatilidad Normativa y Estrategia de Pruebas de Software**

### **Clasificación por Volatilidad Normativa**

* **Stable (Invariable)**: Algoritmo Modulo 11 del RUC, formato DNI, lógica del par Monetario (Amount, Currency).  
* **Regulatory (Cambio Legal)**: Tasa general del IGV (18%), tasa de retención (3%), montos mínimos de bancarización.  
* **Catalog-Driven (Dinámico)**: Catálogo 54 (Detracciones)8, Catálogo 25 (UNSPSC)9, Catálogo 59 (Medios de Pago)10.  
* **Versioned (Evolutivo)**: Estructuras de archivos SIRE, esquemas XML UBL (2.0 vs 2.1)1.

### **Estrategia de Pruebas de Software**

> 1. **Unit Testing de Invariantes**:  
   * Cobertura del 100% en métodos Create, TryCreate y Parse de los Value Objects.  
   * Validación de casos límite (RUCs válidos conocidos, RUCs con dígito verificador incorrecto, cadenas cortas, caracteres alfanuméricos en campos numéricos).  
> 2. **Property-Based Testing**:  
   * Generación aleatoria de miles de iteraciones para CpeSeries y CpeCorrelative mediante librerías como *FsCheck* para verificar la solidez de los límites.  
> 3. **Mutation Testing**:  
   * Pruebas de mutación (*Stryker.NET*) sobre la clase de Modulo 11 para asegurar que cualquier alteración en los coeficientes ponderados cause el fallo de la suite de pruebas.  
> 4. **Golden Master XML / Contract Testing**:  
   * Validación del pipeline completo probando los XML producidos por la Capa Anticorrupción contra archivos "Golden Master" oficialmente convalidados en el entorno de homologación de SUNAT4.

## **17\. Entregables Requireds A \- J**

### **A. Taxonomía Completa de Value Objects**

Clasificación unificada de todos los conceptos del dominio fiscal peruano discriminados entre Value Objects, Entidades, Agregados, DTOs, Servicios y Primitivas Técnicas, garantizando la eliminación de la obsesión por los tipos primitivos.

### **B. Catálogo Maestro de Value Objects con Priorización**

| ID | Value Object | Bounded Context | Tipo .NET 10 | Prioridad | Invariante Principal |
| :---- | :---- | :---- | :---- | :---- | :---- |
| **VO-01** | Ruc | Taxpayer / Shared | readonly record struct | **P0** | 11 dígitos numéricos, prefijos 10/15/17/20, Modulo 11 válido. |
| **VO-02** | IdentityDocument | Shared | readonly record struct | **P0** | Tupla (Tipo Catálogo 06, Número) con Regex según el tipo de documento. |
| **VO-03** | NationalIdentityCard | Shared | readonly record struct | **P0** | Cadena de exactamente 8 dígitos numéricos ASCII (DNI). |
| **VO-04** | CpeSeries | CPE / Billing | readonly record struct | **P0** | 4 caracteres. Formato regulado según el tipo de comprobante (F, B, EG, T). |
| **VO-05** | CpeCorrelative | CPE / Billing | readonly record struct | **P0** | Entero positivo entre 1 y 99,999,999. Relleno con ceros a 8 posiciones. |
| **VO-06** | CpeIdentifier | CPE / Shared | readonly record struct | **P0** | Tupla inmutable (DocumentType, CpeSeries, CpeCorrelative). |
| **VO-07** | Money | Shared Primitives | readonly record struct | **P0** | Monto decimal con escala controlada \+ Código ISO 4217 de moneda. |
| **VO-08** | CurrencyCode | Shared Primitives | readonly record struct | **P0** | Cadena de 3 letras mayúsculas según Catálogo 02 de SUNAT (PEN, USD). |
| **VO-09** | TaxRate | Tax Compliance | readonly record struct | **P0** | Valor decimal mayor o igual a 0 y menor o igual a 1 (e.g., 0.18). |
| **VO-10** | IgvTaxation | Tax Compliance | readonly record struct | **P0** | Tipo Catálogo 07 \+ Tasa (TaxRate) \+ Base (Money) \+ Monto (Money). |
| **VO-11** | ExchangeRate | Billing / Shared | readonly record struct | **P0** | Moneda Origen \!= Moneda Destino, Tasa \> 0, Fecha de cotización válida. |
| **VO-12** | TaxPeriod | Tax Compliance | readonly record struct | **P0** | Cadena YYYYMM de 6 dígitos numéricos. Año \>= 2000, Mes entre 01 y 12\. |
| **VO-13** | Address | Shared Primitives | readonly class | **P0** | Contiene UbigeoCode válido de 6 dígitos \+ Vía normalizada en UTF-8. |
| **VO-14** | UbigeoCode | Shared Primitives | readonly record struct | **P0** | Cadena de exactamente 6 dígitos numéricos registrados en el INEI. |
| **VO-15** | DetractionAccount | Tax Compliance | readonly record struct | **P1** | Cadena de 11 dígitos, inicia con "00", Banco de la Nación. |
| **VO-16** | VehicleLicensePlate | Despatch (GRE) | readonly record struct | **P1** | Cadena de 6 caracteres alfanuméricos, formato MTC tarjeta de propiedad. |
| **VO-17** | GrossWeight | Despatch (GRE) | readonly record struct | **P1** | Cantidad decimal \> 0 \+ Unidad de Medida del Catálogo 03 (KGM, TNE). |
| **VO-18** | SunatProductCode | Billing / Shared | readonly record struct | **P1** | Cadena de 8 dígitos numéricos según norma UNSPSC Catálogo 259. |
| **VO-19** | PaymentTerm | Billing | readonly record struct | **P1** | Tipo (Contado/Crédito) \+ Plazo en días o lista de cuotas (Installment). |
| **VO-20** | Installment | Billing | readonly record struct | **P1** | Número correlativo de cuota \+ Fecha Vencimiento \+ Monto (Money). |

### **C. Value Objects Descartados**

* TaxpayerId: Descartado por duplicidad con Ruc y IdentityDocument.  
* TaxAmount / TaxBase: Descartados como clases VOs explícitas. Son instancias simples de Money.  
* UBLVersionID / CustomizationID: Descartados del dominio fiscal. Son constantes técnicas de transporte XML4.  
* XmlHash / DigitalSignatureThumbprint: Descartados del dominio fiscal. Pertenecen a la capa de infraestructura criptográfica.  
* FiscalAddress / DeliveryAddress: Descartadas como clases separadas. Se unifican en la clase Address.  
* CpeStatus: Descartado como VO; se modela como un enum de estado interno del Aggregate Root CpeInvoice.

### **D. Auditoría y Consolidación de Duplicidades**

Unificación conceptual detallada en la Sección 10, eliminando conceptos redundantes e integrando términos bajo un único estándar terminológico sin ambigüedades.

### **E. Bounded Context Matrix**

Matriz de contexto definida en la Sección 11, delimitando la propiedad y consumo de Value Objects entre los contextos de Padrón, Facturación, Comprobantes Electrónicos, Integración SUNAT, Cumplimiento Tributario y Despacho Logístico.

### **F. Dependency & API Boundary Matrix**

Matriz de visibilidad pública/interna y reglas de dependencia especificada en la Sección 11\.

### **G. Distribución por Paquetes NuGet**

* **EricksonLopez.DomainPrimitives**: Contiene Money, CurrencyCode, Address, ExchangeRate. Sin dependencias externas.  
* **EricksonLopez.Peru**: Contiene Ruc, IdentityDocument, NationalIdentityCard, UbigeoCode, VehicleLicensePlate, SunatProductCode. Depende de EricksonLopez.DomainPrimitives.  
* **EricksonLopez.Fiscal**: Contiene CpeSeries, CpeCorrelative, CpeIdentifier, TaxRate, IgvTaxation, TaxPeriod, DetractionAccount, GrossWeight, PaymentTerm, Installment. Depende de EricksonLopez.Peru.  
* **EricksonLopez.Sunat**: Contiene serializadores UBL XML4, clientes REST SIRE1, adaptadores SOAP y parseadores de CDR. Depende de EricksonLopez.Fiscal.

### **H. Roadmap de Implementación Técnica**

Secuencia de desarrollo por fases definida en la Sección 19, priorizando el núcleo de primitivas y la identificación tributaria P0 antes de la integración con los servicios web de la SUNAT1.

### **I. Registros de Decisiones de Arquitectura (ADRs)**

* **ADR 01**: Eliminación de TaxpayerId en favor de Ruc y IdentityDocument.  
* **ADR 02**: Modelado de Catálogos SUNAT mediante Catalog-Driven Value Objects dinámicos8.  
* **ADR 03**: Desacoplamiento total del modelo de dominio respecto a las estructuras UBL 2.1 XML4.  
* **ADR 04**: Adopción obligatoria de readonly record struct e interfaces ISpanParsable\<T\> en .NET 10 para cero asignaciones de memoria.

### **J. Matriz de Riesgos Arquitectónicos**

Análisis de riesgos por cambios normativos, degradación de desempeño y acoplamiento técnico con sus respectivas estrategias de mitigación detallado en la Sección 21\.

## **18\. Registros de Decisiones de Arquitectura (ADRs)**

### **ADR 01: Eliminación del Identificador Genérico TaxpayerId en favor de Ruc y IdentityDocument**

* **Estado**: Aceptado.  
* **Contexto**: Un identificador tributario genérico de tipo cadena conduce al antipatrón de obsesión por primitivos y diluye las reglas de validación específicas del país.  
* **Decisión**: Se establece el Value Object Ruc como el único identificador para contribuyentes nacionales en el Perú. Para adquirentes no obligados a RUC o extranjeros, se requiere el Value Object compuesto IdentityDocument. Se elimina la clase TaxpayerId.  
* **Consecuencias**: Garantía absoluta de validez del RUC mediante el algoritmo Modulo 11 en el momento de entrada al dominio.

### **ADR 02: Modelado de Catálogos SUNAT mediante Catalog-Driven Value Objects en lugar de Enums C\#**

* **Estado**: Aceptado.  
* **Contexto**: La SUNAT modifica de forma recurrente las Reglas de Validación de los comprobantes de pago y catálogos paramétricos (e.g., adición o eliminación de códigos en el Catálogo 54 de detracciones)8.  
* **Decisión**: Los catálogos paramétricos de la SUNAT se modelan como readonly record struct cuya validación de clave se apoya en un servicio dinámico de catálogos en memoria (ICatalogProvider).  
* **Consecuencias**: El sistema absorbe cambios normativos de la SUNAT mediante actualizaciones de datos sin requerir recompilaciones ni despliegues del código fuente.

### **ADR 03: Desacoplamiento Total del Esquema XML UBL 2.1 respecto al Core de Dominio**

* **Estado**: Aceptado.  
* **Contexto**: El estándar OASIS UBL 2.1 utilizado por la SUNAT está diseñado para transporte e intercambio documental, conteniendo redundancias y verbosidad innecesarias para la lógica interna del negocio4.  
* **Decisión**: Las clases XML y esquemas XSD pertenecen de forma exclusiva a la librería de infraestructura EricksonLopez.Sunat. El dominio opera únicamente con Value Objects puros, utilizando una Capa Anticorrupción (ACL) para la traducción.  
* **Consecuencias**: Independencia tecnológica. Las actualizaciones en las versiones de esquemas de la SUNAT no afectan la arquitectura interna ni el modelo de datos.

### **ADR 04: Adopción de readonly record struct e Interfaces ISpanParsable\<T\> en .NET 10**

* **Estado**: Aceptado.  
* **Contexto**: Un sistema fiscal de facturación electrónica procesa volúmenes elevados de transacciones, donde la instanciación de objetos en el Heap genera latencia por recolección de basura (*GC*).  
* **Decisión**: Todos los Value Objects de tamaño inferior a 64 bytes se implementan obligatoriamente como readonly record struct e implementan ISpanParsable\<T\> e IUtf8SpanParsable\<T\>.  
* **Consecuencias**: Asignaciones de memoria nulas (*Zero-Allocations*) en parsing y serialización, maximizando el rendimiento y la compatibilidad con compilación Native AOT en .NET 10\.

## **19\. Matriz de Riesgos Arquitectónicos y Mitigaciones**

| Riesgo Arquitectónico | Severidad | Impacto en Sistema / Negocio | Estrategia de Mitigación Recomendada |
| :---- | :---- | :---- | :---- |
| **Inoperatividad por Cambios SUNAT** | **Alta** | Rechazo masivo de comprobantes por actualización de reglas de validación o catálogos8. | Implementación de **Catalog-Driven VOs** (ADR 02\) desacoplados del código compilado. |
| **Degradación de Desempeño por Over-Modeling** | **Media** | Latencia en el procesamiento de archivos masivos del SIRE o resúmenes diarios1. | Uso estricto de readonly record struct en .NET 10 con ReadOnlySpan\<char\> (ADR 04). |
| **Acoplamiento Directo a Esquemas UBL** | **Alta** | Refactorizaciones costosas ante migraciones de versión en los esquemas de la SUNAT4. | Capa Anticorrupción (ACL) aislada en la librería EricksonLopez.Sunat (ADR 03). |
| **Inconsistencias Monetarias por Redondeo** | **Crítica** | Rechazos en la SUNAT por descalce de céntimos entre Base Imponible e Impuesto (Error 2014/2021). | Uso del VO Money con redondeo bancario estándar (MidpointRounding.ToEven) a 2 decimales. |
| **Incompatibilidad con Compilación Native AOT** | **Media** | Fallos de ejecución en despliegues optimizados dentro de contenedores de alto rendimiento. | Eliminación de reflexión mediante *Source Generators* de C\# para JSON y BD. |

## **20\. Roadmap de Implementación Técnica**

> 1. **Fase 1: Núcleo de Primitivas y Dominio Peruano (P0)**  
   * Creación del paquete EricksonLopez.DomainPrimitives (Money, CurrencyCode, ExchangeRate, Address).  
   * Creación del paquete EricksonLopez.Peru (Ruc, IdentityDocument, NationalIdentityCard, UbigeoCode).  
   * Pruebas unitarias completas de validación e invariantes algorítmicas (Modulo 11).  
> 2. **Fase 2: Identificación Fiscal y Comprobantes de Pago (P0)**  
   * Creación del paquete EricksonLopez.Fiscal (CpeSeries, CpeCorrelative, CpeIdentifier).  
   * Implementación del motor de formateo y validación de máscaras alfanuméricas del SEE.  
> 3. **Fase 3: Motor Monetario e Impuestos (P0 \- P1)**  
   * Implementación de TaxRate, IgvTaxation y reglas de redondeo bancario sobre Money.  
   * Incorporación de TaxPeriod, DetractionAccount y SunatProductCode8.  
> 4. **Fase 4: Módulo Logístico y Guías de Remisión (P1)**  
   * Incorporación de VehicleLicensePlate, GrossWeight y datos de transporte en EricksonLopez.Peru y EricksonLopez.Fiscal3.  
> 5. **Fase 5: Capa Anticorrupción (ACL) e Integración SUNAT (P1 \- P2)**  
   * Creación del paquete EricksonLopez.Sunat.  
   * Implementación de mappers UBL 2.1 XML4, conectores REST SIRE1 y parseadores de CDR.  
   * Pruebas de integración, pruebas de contrato ("Golden Master XML") y optimización de rendimiento para Native AOT en .NET 10\.

## **21\. Respuesta a la Pregunta Final**

El catálogo mínimo, completo, coherente y arquitectónicamente correcto de Value Objects que debe constituir el núcleo del dominio fiscal peruano en .NET 10 está compuesto por **20 Value Objects indispensables**:

> 1. **Shared Kernel Universal (EricksonLopez.DomainPrimitives)**:  
   * Money: Dominio monetario con precisión decimal y operaciones multimoneda seguras.  
   * CurrencyCode: Mapeo estricto de códigos ISO 4217 del Catálogo 02 de SUNAT (PEN, USD).  
   * Address: Estructura direccional unificada que encapsula ubicación física y código territorial.  
   * ExchangeRate: Conversión tributaria de divisas respaldada por cotizaciones oficiales SBS.  
> 2. **Bounded Context Peruano (EricksonLopez.Peru)**:  
   * Ruc: Identificador fiscal nacional de 11 dígitos validado algorítmicamente mediante Modulo 11\.  
   * IdentityDocument: Value Object compuesto para identificación de adquirentes (DNI, CE, Pasaporte).  
   * NationalIdentityCard: Documento Nacional de Identidad (DNI) de 8 dígitos numéricos ASCII.  
   * UbigeoCode: Identificador geográfico distrital de 6 dígitos oficial del INEI.  
   * VehicleLicensePlate: Identificación de placa de rodaje para transporte de carga (GRE).  
   * SunatProductCode: Clasificación estandarizada de bienes/servicios bajo UNSPSC Catálogo 259.  
> 3. **Bounded Context Fiscal (EricksonLopez.Fiscal)**:  
   * CpeSeries: Validador y formateador de series de comprobantes (Facturas, Boletas, Guías, Notas).  
   * CpeCorrelative: Secuenciador numérico de comprobantes (1 a 8 dígitos con relleno de ceros).  
   * CpeIdentifier: Clave natural compuesta (DocumentType, CpeSeries, CpeCorrelative).  
   * TaxRate: Modificador de tasa de impuesto con operaciones sobre bases imponibles.  
   * IgvTaxation: Estructura de afectación al IGV (Catálogo 07\) que une tasa, base y monto monetario.  
   * TaxPeriod: Identificador de periodo tributario YYYYMM utilizado en SIRE, RVIE y RCE1.  
   * DetractionAccount: Cuenta corriente oficial en el Banco de la Nación para el SPOT.  
   * GrossWeight: Mapeo de peso de carga transportada y unidad de medida oficial (Catálogo 03).  
   * PaymentTerm: Estructura de condición de pago (Contado/Crédito) y control de vencimientos.  
   * Installment: Desglose de cuota individual para comprobantes emitidos al crédito.

### **Ubicación y Permanencia de Conceptos Fuera del Dominio**

* **Permanecen como Entidades / Aggregate Roots**: Taxpayer (Padrón Contribuyente), CpeInvoice (Aggregate Comprobante de Pago), DespatchGuide (Aggregate Dispatch Guide).  
* **Permanecen como DTOs / Modelos de Integración (ACL)**: Mapeadores XML UBL 2.14, JSONs de propuestas del SIRE1, Payloads de CDR de la SUNAT o de la OSE, tickets de envío.  
* **Permanecen como Parámetros de Configuración**: UBLVersionID, CustomizationID, URLs de endpoints de la SUNAT.  
* **Permanecen en la Capa de Criptografía e Infraestructura**: XmlDigest, Thumbprint de certificados digitales, Hashes SHA-256.

#### **Fuentes citadas**

> 1. ¿Cuál es el cronograma oficial del SIRE 2025 y 2026 tras la nueva postergación de SUNAT? \- Seminarios Top, [https://seminariostop.com/blog/sire-2025-cronograma-obligados-sunat/](https://seminariostop.com/blog/sire-2025-cronograma-obligados-sunat/)  
> 2. Facturación Electrónica SUNAT: Guía para Empresas 2026 | DevSprinters Blog, [https://devsprinters.site/blog/facturacion-electronica-sunat-guia-empresas](https://devsprinters.site/blog/facturacion-electronica-sunat-guia-empresas)  
> 3. CPE Sunat, [https://cpe.sunat.gob.pe/](https://cpe.sunat.gob.pe/)  
> 4. Sistemas de emisión | Comprobantes de Pago Electrónicos \- CPE Sunat, [https://cpe.sunat.gob.pe/node/116](https://cpe.sunat.gob.pe/node/116)  
> 5. Guías y Manuales | Comprobantes de Pago Electrónicos \- CPE Sunat, [https://cpe.sunat.gob.pe/guias-y-manuales](https://cpe.sunat.gob.pe/guias-y-manuales)  
> 6. Normas Legales | Comprobantes de Pago Electrónicos \- CPE Sunat, [https://cpe.sunat.gob.pe/node/141](https://cpe.sunat.gob.pe/node/141)  
> 7. Descargas \- NUBEFACT, [https://ayuda.nubefact.com/descargas](https://ayuda.nubefact.com/descargas)  
> 8. SUNAT Modifica Reglas de Validación de CPE y GRE 2025 \- LLB Solutions, [https://llbsolutions.com/es/sunat-implementa-modificaciones-en-las-reglas-de-validacion-de-cpe-y-gre/](https://llbsolutions.com/es/sunat-implementa-modificaciones-en-las-reglas-de-validacion-de-cpe-y-gre/)  
> 9. Códigos de Productos SUNAT | Comprobantes de Pago Electrónico \- CPESUNAT, [https://cpesunat.com/codigo-producto-sunat/](https://cpesunat.com/codigo-producto-sunat/)  
> 10. Catálogo 59 Medios de Pago \- Factpro API, [https://docs.factpro.la/catalogos-sunat/catalogo-59-medios-de-pago](https://docs.factpro.la/catalogos-sunat/catalogo-59-medios-de-pago)