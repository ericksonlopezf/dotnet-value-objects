# **Auditoría y Diseño Exhaustivo de Value Objects para el Ecosistema Fiscal de la República Dominicana**

## **1\. Taxonomía Completa y Clasificación Global del Fiscal Domain Dominicano**

El diseño de un núcleo de dominio orientado a la normativa fiscal de la República Dominicana exige comprender la intersección entre la teoría de *Domain-Driven Design* (DDD) y las exigencias legales dictadas por el Código Tributario (Ley 11-92)1, la Ley 32-23 de Facturación Electrónica2, y las Normas Generales vigentes emitidas por la Dirección General de Impuestos Internos (DGII)4. Un error recurrente en la arquitectura de software empresarial es incurrir en la *Obsesión por los Primitivos* (*Primitive Obsession*), donde cadenas, enteros y decimales crudos transportan reglas implícitas de negocio, o en el *Sobre-modelado* (*Over-modeling*), donde cada atributo de un esquema de datos o de un archivo XML es promovido inductivamente a la categoría de *Value Object* (VO).  
Un *Value Object* legítimo dentro de la arquitectura fiscal debe cumplir con invariantes estructurales precisas: su identidad está definida exclusivamente por sus valores, es intrínsecamente inmutable, garantiza igualdad estructural, no posee un ciclo de vida independiente dentro del almacenamiento persistente, encapsula invariantes estrictas de validación y comportamiento del dominio fiscal, y es conceptualmente independiente de las entidades que lo consumen.  
El marco tributario dominicano no es homogéneo; combina estructuras numéricas rígidas con algoritmos de comprobación matemática como Modulo 10 y Modulo 116, secuencias alfanuméricas de longitud fija3, especificaciones de comprobantes fiscales electrónicos (e-CF)2, y regímenes impositivos mutables regidos por resoluciones administrativas5.  
La taxonomía del dominio fiscal se organiza en siete núcleos fundamentales de modelado:

* **Identificación Fiscal:** Encapsula la identidad tributaria de personas físicas y jurídicas mediante identificadores validados mediante algoritmos comprobatorios o reglas formales de la DGII6.  
* **Secuenciación y Comprobantes Fiscales:** Modela los comprobantes tradicionales (NCF) y los comprobantes fiscales electrónicos (e-CF / e-NCF), garantizando la inmutabilidad de los prefijos, códigos de tipo y correlativos de emisión3.  
* **Documentación Electrónica y Recepción DGII:** Representa la metadata técnica y de trazabilidad fiscal generada en el intercambio de mensajes XML firmados digitalmente, tales como el TrackID, códigos de seguridad y respuestas del web service10.  
* **Cálculos Monetarios e Impositivos:** Modela la precisión decimal, monedas, tasas de cambio y operaciones aritméticas de bases imponibles, exenciones, recargos, intereses y retenciones1.  
* **Temporalidad Fiscal:** Modela los períodos de declaración para formatos como el 606, 607, 608, IT-1 y IR-3, así como las fechas con relevancia legal o de vencimiento impositivo2.  
* **Seguridad Criptográfica Fiscal:** Encapsula los elementos del certificado digital X.509 y el firmado XAdES requerido para validar la autenticidad e integridad del e-CF3.  
* **Clasificación y Codificación Operativa:** Modela unidades de medida, métodos de pago y códigos de actividad económica acordes a los catálogos oficiales de la DGII1.

## **2\. Investigación Normativa y Clasificación de Reglas**

La arquitectura de un software fiscal de producción en la República Dominicana requiere una separación clara de la naturaleza de sus reglas de negocio para evitar que los cambios regulatorios destruyan la estabilidad del código. A continuación se establece la clasificación oficial de las fuentes normativas y el impacto técnico que representan para la modelación del dominio.

| Regla / Dominio | Fuente Normativa y Legal | Nivel de Volatilidad | Impacto en la Software Architecture |
| :---- | :---- | :---- | :---- |
| **Regla Legal** | Código Tributario (Ley 11-92)1, Ley 32-23 de Facturación Electrónica2, Ley 126-02 de Comercio Electrónico y Firma Digital3. | Muy Baja (Años/Décadas) | Define los principios rectores de responsabilidad tributaria, la exigibilidad de comprobantes y las obligaciones de conservación de documentos. Sustenta las invariantes estructurales principales. |
| **Regla Fiscal** | Normas Generales DGII (ej. NG 06-2018 para NCF5, NG 01-2023 para e-CF4, NG 05-2019 para Comprobantes Especiales5), Resoluciones de la DGII. | Media (1 a 3 años) | Establece los catálogos de comprobantes, tasas impositivas vigentes (ITBIS 18%, 16%), porcentajes de retención de ISR y exenciones5. Se modelan mediante *Domain Policies* y *Specifications*. |
| **Regla de Negocio** | Políticas internas de la empresa emisora, condiciones comerciales con clientes, políticas de crédito y descuentos. | Alta (Meses) | Configura cuándo se emite un comprobante a crédito o contado, reglas de aplicación de descuentos comerciales y flujo interno de aprobación previo al timbrado. |
| **Regla Técnica** | Estándar XML XAdES-BES / Enveloped Signature12, Esquema XSD v1.0 e-CF de la DGII11, Codificación UTF-8, Especificaciones de Firma Digital de INDOTEL2. | Baja (3 a 5 años) | Determina la estructura del archivo XML, el cálculo del digest SHA-25612, la canonicación C14N y el embalaje de la firma. Pertenece a la capa de Infraestructura. |
| **Regla de Integración DGII** | Especificación Técnica de Servicios Web de la DGII10, Tokens de Autenticación, Tiempos de Expiración, Reintentos, TrackID10. | Media-Alta (Actualizaciones técnicas de la DGII) | Gobierna el transporte asíncrono y síncrono de mensajes, manejo de errores de conexión y verificación del estado del e-CF4. Implemented en la Capa Anti-Corrupción (ACL). |

## **3\. Auditoría de Duplicidades y Racionalización Semántica**

Antes de definir el catálogo definitivo, es imprescindible realizar una auditoría rigurosa para identificar y eliminar redundancias conceptuales. La proliferación imprecisa de tipos con nombres sutilmente diferentes genera acoplamiento accidental, dispersa la lógica de validación e incrementa la carga cognitiva del desarrollo.

| Conceptos Candidatos a Conflicto | Análisis de Coincidencia Semántica | Decisión Arquitectónica | Justificación Técnica y Fiscal |
| :---- | :---- | :---- | :---- |
| Amount vs Money | Ambos intentan representar valores cuantitativos financieros. Amount carece de divisa, mientras que el cálculo fiscal dominicano requiere asociar explícitamente la moneda ISO 4217 y prevenir operaciones cruzadas sin tasa de cambio. | **Fusionar en Money** | Todo monto en el dominio fiscal dominicano posee implícita o explícitamente una divisa (DOP por defecto, USD o EUR en operaciones especiales). Un decimal aislado (Amount) fomenta errores en transacciones multimoneda. |
| Percentage vs TaxRate | Percentage representa una abstracción matemática pura ![][image1]. TaxRate representa una norma impositiva específica (p. ej., ITBIS 18%, Retención ISR 10%) asociada a una categoría fiscal5. | **Mantener Separados** | Percentage es un VO genérico de SharedKernel. TaxRate es un VO fiscal que encapsula el tipo de impuesto, la vigencia legal y si la tasa es exenta, gravada o reducida5. |
| FiscalPeriod vs TaxPeriod | Sinónimos completos utilizados para representar el ciclo mensual (![][image2]) de reporte tributario2. | **Eliminar TaxPeriod, usar FiscalPeriod** | La literatura técnica de la DGII y los formatos 606/607 utilizan "Fiscal Period"2. Se elimina el anglicismo redundante TaxPeriod. |
| DocumentNumber vs InvoiceNumber | DocumentNumber es excesivamente amplio. InvoiceNumber es una terminología comercial que no refleja la naturaleza legal del NCF o e-NCF2. | **Descartar ambos; usar Ncf / EcfNumber** | La legislación tributaria de RD no valida "números de factura", sino "Voucher Number Fiscal" (NCF) o "Voucher Number Fiscal Electrónico" (e-NCF)2. |
| NCF vs E-NCF | Ambos representan secuencias fiscales autorizadas por la DGII, pero poseen longitudes (11 vs 13 caracteres), estructuras (B vs E) y validaciones sustancialmente divergentes3. | **Mantener Separados (Ncf y EcfNumber)** | La estructura de NCF es B \+ 2 dígitos de tipo \+ 8 secuenciales (11 caracteres)7. La estructura de e-NCF es E \+ 2 dígitos de tipo \+ 10 secuenciales (13 caracteres)3. Unificar mediante herencia o parsing condicional rompe la inmutabilidad y la seguridad tipológica. |
| RNC vs TaxpayerId | RNC es el término legal dominicano para personas jurídicas y físicas registradas6. TaxpayerId es un término genérico internacional. | **Especializar (Rnc y Cedula bajo TaxpayerIdentity)** | El RNC tiene 9 dígitos y se valida con Modulo 116. La Cédula tiene 11 dígitos y se valida con Modulo 106. TaxpayerIdentity actúa como tipo contenedor de alto nivel (*Discriminated Union*). |
| Address vs FiscalAddress | Address representa una ubicación física genérica. FiscalAddress exige la codificación territorial explícita según el catálogo geográfico de la DGII (Provincia, Municipio, Sector). | **Convertir FiscalAddress en Especialización** | Una dirección comercial puede ser un texto libre. La dirección fiscal debe alinearse con la estructura exigida para la validez de comprobantes y registros del contribuyente. |
| CountryCode vs FiscalCountryCode | Código de país estándar ISO 3166-1 alpha-2/alpha-3 frente al código de país utilizado en los reportes de retenciones y pagos al exterior de la DGII13. | **Fusionar en CountryCode (estándar ISO)** | La DGII ha alineado sus estructuras XML de e-CF (como en el tipo 47 de pagos al exterior) con los códigos de país ISO 3166-113. Se evita crear una duplicidad redundante. |
| Hash vs Digest | Conceptos criptográficos idénticos referidos al resultado de la función hash SHA-256 aplicada al XML del e-CF12. | **Fusionar en XmlHash** | El nombre XmlHash denota explícitamente el artefacto procesado dentro del dominio de facturación electrónica12, descartando términos ambiguos. |
| Sequence vs FiscalSequence | Sequence es una abstracción matemática o de infraestructura. FiscalSequence representa el rango de comprobantes autorizados por la DGII (Desde, Hasta, Actual, Vencimiento)8. | **Mantener Separados** | Sequence es un contador interno. FiscalSequence es una estructura de control con reglas de agotamiento y vigencia legal8. |
| Date vs FiscalDate | Date es una abstracción de calendario. FiscalDate representa fechas con trascendencia legal (días hábiles, fechas de corte de retención). | **Descartar FiscalDate; usar DateOnly estructurado en FiscalPeriod o DueDate** | Promover cada fecha a FiscalDate introduce sobre-modelado sin aportar comportamiento diferencial. Se mantiene DateOnly genérico y se encapsulan las reglas en VOs con rol semántico explícito (DueDate, FiscalPeriod). |

## **4\. Catálogo Maestro de Value Objects**

A continuación se detalla el catálogo definitivo de Value Objects estructurado para un entorno de producción de alto rendimiento en .NET 10\. Cada elemento especifica su naturaleza tipológica, sus invariantes y su justificación legal e industrial.

| Campo / Descripción | VO Nombre | Categoría | Área de Dominio | Definición | Tipo Base | Inmutable | Igualdad | Validaciones e Invariantes | Comportamiento / Métodos | Ejemplo RD | Dependencias | DGII | Normativo / Fuente | Cambia Frecuentemente | Domain Primitives | Shared Kernel | Fiscal | Prioridad | Rationale |
| :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- |
| RNC | Rnc | Identificación | Contribuyentes | Identificador tributario de 9 dígitos para personas jurídicas o físicas registradas6. | string | Sí | Valor exacto (9 dígitos) | 9 dígitos numéricos. Algoritmo Modulo 116. No nulo, no espacios. | IsValid(), Format(), ToFormattedString() | "101823123" | Ninguna | Sí | Ley 11-92, Art. 501 | No | Sí | No | Sí | P0 | Garantiza que ningún documento se emita con un RNC jurídicamente inválido6. |
| Cédula | Cedula | Identificación | Contribuyentes | Identificador personal de 11 dígitos para personas físicas dominicanas6. | string | Sí | Valor exacto (11 dígitos) | 11 dígitos numéricos. Algoritmo Modulo 106. No nulo. | IsValid(), Format() | "00112345678" | Ninguna | Sí | Ley de Registro Civil / DGII6 | No | Sí | No | Sí | P0 | Necesario para receptores de e-CF tipo 32 mayores a RD$ 250,000 y compras a informales4. |
| Identificación Tributaria Extranjera | ForeignTaxId | Identificación | Contribuyentes | Identificador fiscal de personas físicas o jurídicas del exterior13. | string | Sí | Valor ordinal (case-insensitive) | Longitud entre 2 y 50 caracteres. Caracteres alfanuméricos y guiones. | ToNormalized() | "US987654321" | Ninguna | Sí | Ley 32-23 (e-CF Tipo 47 / Exportation)2 | No | No | No | Sí | P1 | Requerido para sustentar e-CF de Pagos al Exterior (Tipo 47\) y Exportaciones (Tipo 46\)4. |
| Identidad del Contribuyente | TaxpayerIdentity | Identificación | Contribuyentes | Tipo discriminado que representa un Rnc, Cedula o ForeignTaxId. | struct | Sí | Tipo de identidad \+ Valor | Debe contener exactamente un tipo de identificación válido. | Match(), IsRnc(), IsCedula(), RawValue() | Rnc("101823123") | Rnc, Cedula, ForeignTaxId | Sí | Ley 32-23, Art. 142 | No | No | No | Sí | P0 | Proporciona seguridad tipológica polimórfica en encabezados de e-CF. |
| NCF Tradicional | Ncf | Comprobantes | Comprobantes | Secuencia alfanumérica fija para comprobantes impresos o físicos7. | string | Sí | Cadena ordinal exacta | 11 caracteres. Posición 1 \= 'B'. Posiciones 2-3 \= Tipo válido (01-17). Posiciones 4-11 \= 8 dígitos7. | GetTypeCode(), GetSequenceNumber() | "B0100000001" | Ninguna | Sí | Decreto 254-06, Norma General 06-20185 | No | No | No | Sí | P0 | Imprescindible para operar con proveedores no electrónicos en período de transición. |
| e-NCF / e-CF Number | EcfNumber | Comprobantes | Comprobantes | Secuencia alfanumérica de 13 caracteres para comprobantes electrónicos3. | string | Sí | Cadena ordinal exacta | 13 caracteres. Posición 1 \= 'E'. Posiciones 2-3 \= Tipo e-CF válido (31-47)2. Posiciones 4-13 \= 10 dígitos3. | GetEcfType(), GetSequence(), IsConsumerInvoice() | "E310000000005" | Ninguna | Sí | Ley 32-23, Norma General 01-20232 | No | No | No | Sí | P0 | Núcleo fundamental del sistema de facturación electrónica dominicano2. |
| Secuencia Fiscal | FiscalSequence | Comprobantes | Comprobantes | Rango autorizado por la DGII para emitir NCF o e-NCF8. | struct | Sí | Prefijo \+ Desde \+ Hasta | Desde ![][image3] Hasta. Actual debe estar entre Desde y Hasta. Vencimiento ![][image4] Fecha Actual. | CanEmit(), Next(), IsExhausted(), Remaining() | E31 / 1 a 1000 | DueDate | Sí | Norma General 06-2018 / DGII8 | No | No | No | Sí | P0 | Evita la emisión e-CF fuera de rango o vencida, previniendo rechazos de la DGII8. |
| Código de Seguridad e-CF | EcfSecurityCode | e-CF | Facturación Electrónica | Cadena de 6 caracteres generada a partir de los datos firmados del e-CF10. | string | Sí | Cadena exacta de 6 caracteres | 6 caracteres alfanuméricos en mayúsculas extraídos del hash/firma según la regla DGII10. | ToQrUrl() | "A1B2C3" | Ninguna | Sí | Especificación Técnica e-CF v1.0 DGII10 | No | No | No | Sí | P0 | Requerido en la Representación Impresa y código QR para validación rápida3. |
| TrackID | TrackId | e-CF | Facturación Electrónica | Código único de recepción generado por la DGII al recibir un e-CF10. | string | Sí | Cadena ordinal exacta | Formato alfanumérico no vacío. Retornado por la API de la DGII10. | IsPending(), ToString() | "a1b2c3d4-e5f6-7890" | Ninguna | Sí | Especificación Técnica API DGII10 | No | No | No | Sí | P0 | Clave sintáctica obligatoria para la consulta asíncrona del estado del comprobante4. |
| Moneda | CurrencyCode | Dinero | Contabilidad | Código alfabético estándar de la divisa utilizada. | string | Sí | Código ISO de 3 letras | Exactamente 3 letras mayúsculas ISO 4217 (ej. DOP, USD, EUR). | IsDop(), IsForeign() | "DOP" | Ninguna | No | Estándar ISO 4217 / DGII | No | Sí | Sí | No | P0 | Previene la combinación errónea de importes en monedas distintas sin conversión. |
| Monto Monetario | Money | Dinero | Contabilidad | Valor monetario de precisión finita con su divisa asociada. | struct | Sí | Monto decimal \+ CurrencyCode | Escala hasta 4 decimales. Redondeo Bancario (*Half-Even*). | Add(), Subtract(), Multiply(), Percentage() | 1500.00 DOP | CurrencyCode | Sí | Código Tributario, Art. 300 | No | Sí | Sí | No | P0 | Representa valores absolutos sin riesgo de pérdida de precisión por punto flotante. |
| Tasa de Cambio | ExchangeRate | Dinero | Contabilidad | Tasa de conversión oficial publicada por el Banco Central o acordada. | struct | Sí | Divisa Origen \+ Divisa Destino \+ Valor | Tasa ![][image5]. Moneda Origen ![][image6] Moneda Destino. Escala de 4 decimales. | Convert(), Invert() | 1 USD \= 60.2500 DOP | CurrencyCode | Sí | Ley 11-92, Norma General de Divisas | Sí | No | No | Sí | P1 | Required cuando la transacción se negocia en moneda extranjera para llevar a DOP. |
| Tasa Impositiva | TaxRate | Impuestos | Impuestos | Porcentaje o alícuota aplicable a una base imponible según tipo de impuesto5. | struct | Sí | Tipo Impuesto \+ Porcentaje | Porcentaje entre ![][image7] y ![][image8]. Tipos: ITBIS, ISR, ISC, Retención5. | CalculateTax(), IsExempt() | ITBIS 18.00% | Percentage | Sí | Código Tributario / Ley 32-232 | Sí | No | No | Sí | P0 | Modela la alícuota legal protegiendo el cálculo contra tasas no contempladas por la DGII. |
| Porcentaje Abstracto | Percentage | Dinero | Contabilidad | Representación de una proporción matemática entre 0 y 100\. | decimal | Sí | Valor decimal | Valor ![][image9]. Escala fija de 4 decimales. | ApplyTo(), FromFraction() | 18.00% | Ninguna | No | Matemática genérica | No | Sí | Sí | No | P0 | Proporciona primitivas reutilizables para cualquier cálculo de prorrateo o descuento. |
| Fiscal Period | FiscalPeriod | Fechas | Declaraciones | Período mensual de presentación de obligaciones (AAAAMM)2. | struct | Sí | Año \+ Mes | Año entre 2000 y 2100\. Mes entre 1 y 12\. | GetFirstDay(), GetLastDay(), Next(), Previous() | 202603 | Ninguna | Sí | Ley 11-92 / Formatos 606 y 6072 | No | No | No | Sí | P0 | Estructura clave para la agrupación de libros de compras, ventas y declaraciones2. |
| Due Date Fiscal | DueDate | Fechas | Comprobantes | Fecha límite para el pago o validez legal de una secuencia o documento. | DateOnly | Sí | Valor DateOnly | Fecha válida en el calendario. Debe ser mayor o igual a la fecha de emisión. | IsExpired(), DaysRemaining() | 2026-12-31 | Ninguna | Sí | Reglamento 254-06 / Ley 32-232 | No | No | No | Sí | P1 | Controla la expiración de NCF y autorizaciones temporales de secuencias8. |
| Dirección Fiscal Normalizada | FiscalAddress | Ubicaciones | Contribuyentes | Domicilio legal del contribuyente acorde al catálogo territorial de la DGII. | struct | Sí | Código Provincia \+ Municipio \+ Sector \+ Calle | Los códigos de provincia y municipio deben existir en el catálogo DGII. Calle no vacía. | ToFormattedAddress() | DN, Sto Dgo, Piantini | Ninguna | Sí | Norma General sobre Registro RNC | No | No | No | Sí | P2 | Evita rechazos en el padrón de contribuyentes y en la emisión de e-CF. |
| Actividad Económica | EconomicActivityCode | Contribuyentes | Contribuyentes | Código oficial de la actividad comercial registrada ante la DGII. | string | Sí | Cadena de 6 dígitos | 6 dígitos numéricos registrados en el catálogo CIIU de la DGII. | GetCategory() | "620101" | Ninguna | Sí | Catálogo de Actividades DGII | Sí | No | No | Sí | P2 | Valida si un contribuyente está facultado para emitir ciertos tipos de e-CF especiales5. |
| Unidad de Medida | UnitOfMeasure | Productos | Inventario | Código normalizado de unidad física o de servicio para ítems de e-CF. | string | Sí | Código exacto en catálogo | Debe pertenecer al catálogo de unidades habilitado por la DGII/UN/ECE. | IsService(), IsPhysical() | "KGM", "UN" | Ninguna | Sí | Especificación XML e-CF DGII15 | No | No | No | Sí | P1 | Garantiza la validez del esquema XML en el desglose de ítems comercializados. |
| Hash del XML | XmlHash | Criptografía | Facturación Electrónica | Hash SHA-256 codificado en Base64/Hex del XML firmado12. | string | Sí | Cadena exacta en Base64 | 44 caracteres en Base64 (SHA-256) o 64 hexadecimales12. | ToHex(), ToBase64() | "a7f8...==" | Ninguna | Sí | Especificación de Firma XAdES DGII12 | No | No | No | Sí | P0 | Garantiza la integridad del documento electrónico y la trazabilidad legal3. |
| Thumbprint del Certificado | DigitalCertificateThumbprint | Criptografía | Facturación Electrónica | Huella digital X.509 del certificado digital utilizado para la firma XAdES3. | string | Sí | Cadena Hexadecimal | 40 caracteres hexadecimales (SHA-1) o 64 (SHA-256). | ToCleanString() | "A1B2C3..." | Ninguna | Sí | INDOTEL / DGII Firma Digital2 | No | No | No | Sí | P1 | Vincula inequívocamente la firma del e-CF con el emisor autorizado por INDOTEL2. |
| Código de Respuesta DGII | DgiiResponseCode | e-CF | Facturación Electrónica | Estado de procesamiento del e-CF retornado por la DGII10. | int | Sí | Valor numérico | Código numérico oficial (ej. 0 \= Aceptado, 1 \= Rechazado, 2 \= Condicional)10. | IsAccepted(), IsRejected() | 0 | Ninguna | Sí | Especificación Web Services DGII10 | Sí | No | No | Sí | P0 | Determina el flujo de negocio del comprobante (aceptación, anulación o reintento)4. |
| Payment Method Fiscal | PaymentMethodCode | Pagos | Contabilidad | Código oficial de la modalidad de pago empleada en la transacción1. | string | Sí | Código numérico de 2 dígitos | Códigos válidos: 01 (Efectivo), 02 (Cheque/Transferencia), 03 (Tarjeta), 04 (Crédito), etc.1 | IsCash(), IsCredit() | "01" | Ninguna | Sí | Catálogo e-CF DGII / Tabla Pagos1 | No | No | No | Sí | P1 | Determina si el e-CF es al contado o a crédito, afectando la exigibilidad fiscal14. |
| ID de Correlación | CorrelationId | Auditoría | Auditoría | Guid de trazabilidad única a través de la infraestructura del sistema. | Guid | Sí | Valor Guid de 128 bits | Guid no vacío (Guid.Empty no permitido). | NewGuid(), ToString() | "f47ac10b..." | Ninguna | No | Patrón de Observabilidad | No | Sí | Sí | No | P0 | Permite rastrear la transmisión del comprobante a través de buses de eventos y logs. |

## **5\. Conceptos Descartados y Alternativas de Modelado**

Para prevenir el sobre-modelado dentro del ecosistema fiscal, diversos conceptos fueron evaluados y explícitamente descartados como Value Objects. A continuación se presentan las razones técnicas y normativas, acompañadas de su alternativa arquitectónica adecuada.

| Concepto Descartado | Categoría Inicial Tentativa | Alternativa Correcta de Modelado | Justificación de Rechazo como Value Object |
| :---- | :---- | :---- | :---- |
| DgiiClient / DgiiSupplier | Value Object | **Agregado / Entidad (Taxpayer)** | Poseen un ciclo de vida propio, cambian de estado tributario (Activo, Suspendido, Dado de Baja), modifican su razón social y sus obligaciones fiscales a lo largo del tiempo. No cumplen la regla de inmutabilidad ni de igualdad por valor. |
| EcfDocument / FacturaElectronica | Value Object | **Raíz de Agregado (EcfAggregate)** | Representa el documento fiscal completo. Posee una máquina de estados compleja (Borrador, Firmado, Enviado, Aceptado, Rechazado, Anulado)4 y muta a medida que la DGII responde. Un VO no puede sufrir transiciones de estado interno. |
| TaxCatalog | Value Object | **Repositorio de Consulta / Servicio de Dominio** | Los catálogos de la DGII (códigos de retención, provincias, tipos de ingresos) son conjuntos de datos de referencia que cambian por disposiciones administrativas. Tratar un catálogo como VO confunde datos estáticos con tipos de dominio. |
| XmlDocument / XmlStream | Value Object | **Artefacto Técnico de Infraestructura / DTO** | Es una representación física del mensaje de transporte serializado2. Introducir clases de manipulación XML dentro del modelo de dominio viola el principio de independencia de infraestructura. |
| DgiiEnvironment | Value Object | **Enumeración (enum DgiiEnvironment) / Configuración** | Es una constante que define el destino de la red (Pre-Certificación, Certificación, Producción)11. Carece de reglas de validación o comportamiento de dominio dinámico. |
| TaxReturn (606, 607, 608, IT-1) | Value Object | **Agregado / Estructura de Exportation (DTO)** | Los reportes mensualmente presentados son agregaciones temporales de transacciones fiscales generadas bajo un período específico2. Su naturaleza es la de un informe estructurado con identidad definida por su período y RNC emisor2. |
| PuntoEmision / Establecimiento | Value Object | **Entidad dentro del Agregado de Empresa** | Aunque poseen códigos numéricos estables (ej. "001", "002"), forman parte de la jerarquía física de la empresa y pueden activarse, desactivarse o cambiar de dirección. |

## **6\. Arquitectura de Distribución en Librerías y Bounded Contexts**

Para garantizar que los tipos fiscales dominicanos no contaminen el núcleo del sistema empresarial de propósito general, se define una estrategia de modularización jerárquica basada en librerías de .NET 10:

* **EricksonLopez.DomainPrimitives (Nivel 0 \- Genérico Universal):** Contiene interfaces base (IValueObject), primitivas de validación y el patrón Result\<T\> base. Solo depende del BCL de .NET 10\.  
* **EricksonLopez.SharedKernel (Nivel 1 \- DDD Transversal):** Define tipos financieros y de trazabilidad universales como Money, CurrencyCode, Percentage y CorrelationId.  
* **EricksonLopez.Fiscal (Nivel 2 \- Fiscal Domain Dominicano Base):** Modela la normativa tributaria dominicana tradicional: Rnc, Cedula, TaxpayerIdentity, Ncf, FiscalPeriod, DueDate y TaxRate2.  
* **EricksonLopez.ECF (Nivel 3 \- Facturación Electrónica Especializada):** Encapsula los conceptos de la Ley 32-23: EcfNumber, TrackId, XmlHash, EcfSecurityCode y DigitalCertificateThumbprint2.  
* **EricksonLopez.Fiscal.Application (Nivel 4 \- Capa de Aplicación):** Orquesta los casos de uso, firmas de documentos y flujos de trabajo sin exponer detalles de persistencia.  
* **EricksonLopez.Fiscal.Infrastructure (Nivel 4 \- Capa de Infraestructura):** Implementa la serialización XML nativa, formateadores de base de datos (EF Core / Dapper) y clientes de comunicación con la DGII10.

## **7\. Matriz de Bounded Contexts y Capa Anti-Corrupción (ACL)**

Cada *Bounded Context* en una arquitectura impulsada por el dominio posee requerimientos específicos sobre qué Value Objects necesita manipular. La reutilización indiscriminada de un VO en contextos inadecuados genera acoplamiento entre módulos.

| Value Object | Taxpayer | Fiscal | Billing | ECF | Accounting | Payments | Inventory | Sales | Purchases | Payroll | TaxReturns | Audit |
| :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- |
| Rnc | **Propietario** | Compartido | Compartido | Compartido | Consulta | Consulta | N/A | Consulta | Compartido | Consulta | Compartido | Consulta |
| Cedula | **Propietario** | Compartido | Compartido | Compartido | N/A | N/A | N/A | Consulta | Compartido | Compartido | Compartido | Consulta |
| Ncf | N/A | Compartido | **Propietario** | N/A | Compartido | Consulta | N/A | Compartido | Compartido | N/A | Compartido | Consulta |
| EcfNumber | N/A | Compartido | **Propietario** | **Propietario** | Compartido | Consulta | N/A | Compartido | Compartido | N/A | Compartido | Consulta |
| TrackId | N/A | N/A | Consulta | **Propietario** | N/A | N/A | N/A | N/A | N/A | N/A | N/A | Compartido |
| Money | N/A | Compartido | Compartido | Compartido | **Propietario** | Compartido | Compartido | Compartido | Compartido | Compartido | Compartido | Compartido |
| TaxRate | N/A | **Propietario** | Compartido | Compartido | Compartido | N/A | Consulta | Compartido | Compartido | Consulta | Compartido | Consulta |
| FiscalPeriod | N/A | Compartido | Compartido | Compartido | Compartido | N/A | N/A | Compartido | Compartido | Compartido | **Propietario** | Compartido |
| XmlHash | N/A | N/A | N/A | **Propietario** | N/A | N/A | N/A | N/A | N/A | N/A | Compartido | **Propietario** |

### **Capa Anti-Corrupción (Anti-Corruption Layer \- ACL)**

Para proteger el núcleo del dominio de las variaciones externas en las APIs de la DGII, proveedores de timbrado, pasarelas de pago y sistemas ERP de terceros, se definen fronteras de traducción mediante la Capa Anti-Corrupción.  
La ACL recibe cadenas crudas o DTOs XML/JSON desde los servicios web de la DGII o sistemas externos10. La ACL ejecuta el parseo invocando los métodos TryCreate de los Value Objects. Si la validación tiene éxito, la ACL entrega una instancia fuertemente tipada del VO al modelo de dominio. Si la validación falla, la ACL traduce el error en un ValidationError y detiene el procesamiento antes de impactar las entidades del dominio.

## **8\. Matriz de Dependencias y Fronteras de Paquetes**

Para preservar la pureza arquitectónica, la matriz de referencias entre librerías establece estrictamente qué paquetes tienen permitido consumir a otros, evitando dependencias circulares y acoplamiento indebido.

| Librería Origen | Depende de DomainPrimitives | Depende de SharedKernel | Depende de Fiscal | Depende de ECF | Depende de Infrastructure |
| :---- | :---- | :---- | :---- | :---- | :---- |
| EricksonLopez.DomainPrimitives | **Origen Base** | Prohibido | Prohibido | Prohibido | Prohibido |
| EricksonLopez.SharedKernel | **Permitido** | **Origen Base** | Prohibido | Prohibido | Prohibido |
| EricksonLopez.Fiscal | **Permitido** | **Permitido** | **Origen Base** | Prohibido | Prohibido |
| EricksonLopez.ECF | **Permitido** | **Permitido** | **Permitido** | **Origen Base** | Prohibido |
| EricksonLopez.Fiscal.Application | **Permitido** | **Permitido** | **Permitido** | **Permitido** | Prohibido |
| EricksonLopez.Fiscal.Infrastructure | **Permitido** | **Permitido** | **Permitido** | **Permitido** | **Origen Base** |

## **9\. Especificación de API Conceptual, Result Pattern, Serialización y Performance (.NET 10 & Native AOT)**

### **Estrategia de Selección de Tipos en .NET 10**

En .NET 10 y entornos compilados mediante *Native AOT*, la asignación de memoria en el *Heap* (*allocations*) debe minimizarse para alcanzar latencias ultra-bajas en el procesamiento masivo de facturación electrónica.

* **readonly record struct:** Es la opción predeterminada para todos los Value Objects pequeños y medianos (tamaño ![][image10] bytes). Proporciona semántica de inmutabilidad garantizada por el compilador, igualdad de miembros por valor optimizada, sintaxis concisa y **cero alocaciones en el Heap** al pasarse por valor o mediante referencias de solo lectura (in / ref readonly).  
* **readonly struct:** Utilizada cuando se requiere control estricto sobre el diseño de memoria (*struct layout*), inicialización personalizada sin constructores sintéticos, e implementación explícita de interfaces de formateo o *parsing*.  
* **sealed class (Excepción de Diseño):** Solo reservada para Value Objects raros de gran tamaño o complejidad donde el costo de copia de la estructura en la pila (*stack copy overhead*) exceda los 64 bytes. Ningún VO del núcleo fiscal dominicano requiere esta excepción.

### **Optimizaciones para Zero-Allocation y Parsing Eficiente**

Todos los Value Objects seleccionados implementan las interfaces unificadas de procesamiento numérico y de texto de .NET 10:

* IParsable\<TSelf\>: Para deserialización tipada desde cadenas de texto.  
* ISpanParsable\<TSelf\>: Permite parsear segmentos de memoria de texto (ReadOnlySpan\<char\>) **sin alocar memoria intermedia** en el Heap.  
* IUtf8SpanParsable\<TSelf\>: Permite parsear directamente desde *buffers* de red o *streams* JSON/XML codificados en UTF-8 (ReadOnlySpan\<byte\>), eliminando la necesidad de transcodificar a string de C\#.  
* IFormattable / ISpanFormattable / IUtf8SpanFormattable: Para dar formato a los valores directamente en buffers de salida.

El proceso de parseo interno opera recibiendo un ReadOnlySpan\<char\> (por ejemplo, "E310000000005"). La validación verifica la longitud (13 caracteres) y la presencia del prefijo 'E'3 utilizando operaciones en *Stack* sobre el *Span*, sin invocar Substring(). Al finalizar, se instancia directamente el struct en la pila de ejecución, garantizando 0 bytes de alocación en el Heap.

### **Integración con el Patrón Result**

El dominio fiscal prohíbe el uso de excepciones (throw Exception) para controlar los errores de validación y de reglas normativas. Las excepciones en .NET destruyen el rendimiento debido a la captura del *Stack Trace* y generan flujos de control impredecibles. Se adopta un patrón Result\<TValue, TError\> estructurado.  
Cuando se intenta crear un Value Object, las validaciones de invariantes determinan el resultado. Si los datos son válidos, se retorna Result.Success conteniendo la instancia inmutable. Si se violan las invariantes, se retorna Result.Failure adjuntando un ValidationError.  
Los errores se categorizan explícitamente en:

* ValidationError.Format: Cadenas fuera de patrón (ej. RNC con letras o longitud distinta a 9 u 11 dígitos)6.  
* ValidationError.Regulatory: Incumplimiento de norma (ej. Emitir e-CF tipo 31 a un receptor no registrado)2.  
* ValidationError.Range: Montos negativos en bases imponibles o secuencias agotadas8.  
* ValidationError.Context: Fechas de emisión incongruentes con el período fiscal abierto2.

### **Estrategia Multicapa de Serialización**

Un único Value Object debe poder representarse en múltiples formatos según el plano de ejecución, sin acoplar la clase del dominio a librerías externas.

| Plano de Ejecución | Formato de Salida | Mecanismo de Mapeo y Serialización |
| :---- | :---- | :---- |
| **Dominio Interno** | Instancia de Struct (readonly record struct) | Tipado fuerte en memoria. Sin serialización. |
| **Intercambio e-CF (DGII)** | XML Estándar v1.011 | Mapeadores dedicados en Infrastructure utilizando System.Xml.XmlWriter nativo o transformadores con anotaciones XML específicas. El VO expone ToFiscalXmlValue(). |
| **Persistencia (PostgreSQL / EF Core)** | Columnas Primitivas (VARCHAR, NUMERIC, DATE) | Configuración de HasConversion en EF Core o *Type Handlers* en Dapper. Los VOs se descomponen en tipos primitivos relacionales. |
| **APIs REST / JSON Intermedias** | JSON de alto rendimiento (System.Text.Json) | Converters genéricos de JSON (JsonConverter\<T\>) que leen y escriben utilizando directamente las interfaces IUtf8SpanParsable\<T\> e IUtf8SpanFormattable. |

## **10\. Versionado Normativo y Resiliencia ante Cambios de la DGII**

Uno de los mayores riesgos en la construcción de sistemas tributarios es la mutabilidad de las leyes y normas generales emitidas por la administración tributaria. Si un Value Object encapsula una regla de tasa impositiva o un catálogo de códigos que la DGII modifica mediante una nueva resolución, el código del dominio queda obsoleto y exige un redespliegue de la infraestructura.

### **Desacoplamiento de Reglas Estables vs Reglas Volátiles**

* **Invariantes Estables (Encapsuladas en VOs):** Reglas de estructura matemática y sintáctica que permanecen inmutables en el tiempo, tales como la estructura de la Cédula con Modulo 106, la longitud del e-NCF en 13 caracteres3, y la precisión impositiva decimal.  
* **Reglas Volátiles (Extraídas a Domain Policies y Specifications):** Valores de tasas cambiantes (ej. cambio de la tasa general de ITBIS)5, límites monetarios sujetos a inflación (ej. el umbral de RD$ 250,000 para e-CF tipo 32 de consumo)4, y la activación de nuevos tipos de comprobantes2.

Las combinaciones de reglas volátiles se gestionan mediante el patrón *Specification* y *Domain Policy*, alimentados por tablas de configuración versionadas en el tiempo, permitiendo que los VOs sigan siendo contenedores inmutables válidos.

## **11\. Estrategia de Pruebas y Aseguramiento de Calidad**

La corrección de los Value Objects del núcleo fiscal debe ser demostrada matemáticamente y mediante pruebas automatizadas estructuradas en cuatro niveles:

* **Pruebas Unitarias Deterministas (Unit Tests):** Verificación de casos bordes mediante validación de RNCs reales y deliberadamente alterados6, comprobación de algoritmos Modulo 10 y Modulo 116, y formateo de cadenas.  
* **Pruebas Basadas en Propiedades (Property-Based Testing con FsCheck):** Generación automática de miles de combinaciones de importes monetarios para comprobar asociatividad, conmutatividad y conservación del redondeo bancario en Money. Comprobación de que FiscalSequence jamás emita un valor fuera de rango8.  
* **Pruebas de Mutación (Mutation Testing con Stryker.NET):** Modificación deliberada de los operadores lógicos en las validaciones internas de los VOs (ej. cambiar un \< por un \<= en la validación del rango de FiscalPeriod). El *Mutation Score* debe ser del 100% en las librerías EricksonLopez.Fiscal y EricksonLopez.ECF.  
* **Pruebas de Contrato (Contract Testing):** Validación de que la salida serializada de EcfNumber, EcfSecurityCode y XmlHash coincida exactamente con las especificaciones del esquema XSD v1.0 publicado por la DGII11.

## **12\. Roadmap y Matriz de Priorización de Implementación**

La implementación del sistema fiscal debe realizarse de forma evolutiva y ordenada por dependencias críticas.

| Fase / Prioridad | Nivel de Madurez | Value Objects Incluidos | Justificación de Fase |
| :---- | :---- | :---- | :---- |
| **P0** | **Core Imprescindible** | Rnc, Cedula, TaxpayerIdentity, Ncf, EcfNumber, FiscalSequence, Money, CurrencyCode, Percentage, TaxRate, FiscalPeriod, TrackId, EcfSecurityCode, XmlHash, DgiiResponseCode, CorrelationId. | Sin estos VOs es imposible emitir, recibir, validar o declarar un solo comprobante fiscal en la República Dominicana2. |
| **P1** | **Altamente Recomendado** | ForeignTaxId, ExchangeRate, DueDate, UnitOfMeasure, PaymentMethodCode, DigitalCertificateThumbprint. | Necesarios para completar el ciclo de facturación comercial compleja, facturas a crédito, ventas multimoneda y retenciones12. |
| **P2** | **Especializado** | FiscalAddress, EconomicActivityCode. | Requeridos para módulos de auditoría avanzada, validación del padrón RNC extendido e integración con el portal gubernamental. |
| **P3** | **Optional / Auxiliar** | Metadatos de geolocalización o etiquetas personalizadas de integración con ERPs de terceros. | Útiles para aplicaciones específicas de punto de venta (POS) o logística, pero ajenos al núcleo fiscal indispensable. |

## **13\. Architectural Decision Records (ADRs)**

### **ADR-001: Racionalización y Unificación de Tipos Monetarios y de Ubicación**

* **Estatus:** Aprobado.  
* **Contexto:** Se detectó el riesgo de duplicidad entre Amount y Money, así como entre Address y FiscalAddress.  
* **Decisión:**  
  1. Eliminar Amount. Adoptar Money como readonly record struct inmutable conteniendo un valor decimal de alta precisión y un CurrencyCode ISO 4217\.  
  2. Especializar FiscalAddress para exigir la estructura territorial oficial de la DGII (Provincia, Municipio, Sector), manteniendo Address como una estructura general de texto libre en la capa comercial.  
* **Consecuencias:** Se elimina la ambigüedad en los cálculos multimoneda y se garantiza la validez territorial en los reportes tributarios.

### **ADR-002: Separación Estricta entre Fiscal Domain Tradicional y Facturación Electrónica (Ley 32-23)**

* **Estatus:** Aprobado.  
* **Contexto:** Mezclar la lógica de NCF tradicional (11 caracteres)7 con e-NCF (13 caracteres)3 dentro del mismo Value Object provocaba condicionales difusos y violaba el principio de responsabilidad única.  
* **Decisión:** Crear dos paquetes separados: EricksonLopez.Fiscal (contiene Ncf tradicional y reglas tributarias universales) y EricksonLopez.ECF (contiene EcfNumber, TrackId, XmlHash y firma digital)2.  
* **Consecuencias:** Proyectos que solo requieran gestionar comprobantes tradicionales o reportes 606/6072 no arrastran dependencias pesadas de facturación electrónica ni esquemas XML.

### **ADR-003: Adopción Obligatoria de readonly record struct y Zero-Allocations para .NET 10 y Native AOT**

* **Estatus:** Aprobado.  
* **Contexto:** El procesamiento masivo de facturas electrónicas en sistemas POS y ERPs puede generar millones de alocaciones de corta vida en el Heap, disparando las pausas por *Garbage Collection*.  
* **Decisión:** Todos los Value Objects del ecosistema se implementan como readonly record struct o readonly struct, aprovechando las interfaces ISpanParsable\<T\> e IUtf8SpanParsable\<T\> de .NET 10\.  
* **Consecuencias:** Rendimiento óptimo, compatibilidad completa con compilación *Native AOT* y reducción del consumo de memoria a cero alocaciones en el parseo y formateo de comprobantes.

### **ADR-004: Desacoplamiento de Catálogos Cambiantes de la DGII mediante Domain Policies**

* **Estatus:** Aprobado.  
* **Contexto:** Las tasas impositivas y catálogos de comprobantes de la DGII pueden ser modificados por leyes o normas generales futuras2.  
* **Decisión:** Los VOs no codifican de forma rígida (*hardcoded*) las listas de tasas o catálogos mutables. Los VOs aseguran la validez sintáctica y estructural, mientras que la validez semántica temporal se delega a servicios de dominio e inyección de *Domain Policies*.  
* **Consecuencias:** El núcleo de dominio permanece inalterado ante cambios reglamentarios de la DGII.

### **ADR-005: Eliminación de Excepciones de Dominio en Favor de Result\<T\> y Validaciones por Repositorio**

* **Estatus:** Aprobado.  
* **Contexto:** El uso de excepciones para errores de validación de entradas de datos daña el rendimiento y complica la gestión de errores en flujos masivos.  
* **Decisión:** Toda fábrica o método de parseo de Value Objects expondrá TryCreate(...) o retornará un Result\<TValue, ValidationError\>.  
* **Consecuencias:** Flujos de código explícitos, seguros y sin penalización de rendimiento por trazado de pila de excepciones.

## **14\. Architecture Decision Summary**

Las decisiones de diseño de la arquitectura fiscal presentan un carácter irreversible o de muy alto costo de modificación posterior:

* **Inmutabilidad Absoluta y Asignación en Stack:** La decisión de modelar los VOs como estructuras de solo lectura (readonly record struct) impide que en el futuro se introduzcan métodos mutadores o herencia de clases. Cambiar esto requeriría reescribir toda la capa de persistencia y serialización del framework.  
* **Separación Física de Paquetes (Fiscal vs ECF):** La frontera entre el marco fiscal general y la facturación electrónica queda congelada. Ningún tipo de e-CF podrá ser movido hacia el paquete Fiscal base, evitando la contaminación cruzada.  
* **Uso Exclusivo del Patrón Result:** Ningún Value Object del núcleo expondrá constructores públicos que lancen excepciones en caso de datos inválidos. Las factorías privadas forzarán el uso de Result\<T\> o TryCreate.  
* **Tipado Explícito de Comprobantes:** Queda estrictamente prohibido representar un NCF o e-NCF como un simple string dentro de la capa de dominio o aplicación. La firma de los métodos exige el uso del VO correspondiente.

## **15\. Análisis de Riesgos Arquitectónicos**

El diseño presentado mitiga explícitamente los principales riesgos en el desarrollo de software fiscal:

* **Riesgo de Sobre-modelado (*Over-modeling*):** Se han descartado conceptos puramente informativos o dependientes de contexto (como respuestas completas de la DGII o nodos XML secundarios), manteniéndolos en DTOs de infraestructura10.  
* **Riesgo de Obsesión por los Primitivos (*Primitive Obsession*):** Previene que RNCs o NCFs se procesen como cadenas de texto crudas, lo que permitiría que datos inválidos o mal formateados atraviesen la capa de aplicación6. Se imponen VOs fuertemente tipados con validadores automáticos en la frontera de entrada del sistema.  
* **Riesgo de Acoplamiento Accidental con la DGII:** Previene acoplar las clases del dominio a las estructuras autogeneradas del esquema XSD de la DGII11. Se utiliza una Capa Anti-Corrupción (ACL) y mapeadores de infraestructura que traducen entre los XMLs de la DGII y los VOs del dominio.  
* **Riesgo de Contaminación del Kernel Compartido (*Shared Kernel Pollution*):** Evita incluir abstracciones específicas de la República Dominicana (como el RNC o el e-NCF)3 dentro de librerías genéricas de arquitectura de software. Se mantiene una jerarquía de paquetes de cuatro niveles (DomainPrimitives ![][image11] SharedKernel ![][image11] Fiscal ![][image11] ECF).

## **16\. Respuesta a la Pregunta de Oro**

**¿Cuál sería el conjunto mínimo, completo y arquitectónicamente correcto de Value Objects que debería constituir el núcleo de dominio de un framework fiscal para República Dominicana, y cuáles deberían quedar fuera de dicho núcleo?**  
El conjunto **mínimo, completo y arquitectónicamente correcto** que debe constituir el núcleo del dominio fiscal dominicano está compuesto por exactamente **24 Value Objects**, clasificados por su frontera funcional:

### **Núcleo Indispensable del Fiscal Domain (24 Value Objects)**

* **Subdominio de Identificación (4 VOs):** Rnc, Cedula, ForeignTaxId, TaxpayerIdentity.  
* **Subdominio de Comprobantes y Secuenciación (3 VOs):** Ncf, EcfNumber, FiscalSequence.  
* **Subdominio de Facturación Electrónica (Ley 32-23) (5 VOs):** EcfSecurityCode, TrackId, XmlHash, DigitalCertificateThumbprint, DgiiResponseCode2.  
* **Subdominio Monetario y Financiero (5 VOs):** Money, CurrencyCode, ExchangeRate, Percentage, TaxRate.  
* **Subdominio Temporo-Fiscal (2 VOs):** FiscalPeriod, DueDate.  
* **Subdominio de Operaciones y Catálogos Fiscales (4 VOs):** FiscalAddress, EconomicActivityCode, UnitOfMeasure, PaymentMethodCode1.  
* **Subdominio de Observabilidad (1 VO):** CorrelationId.

### **Conceptos Excluidos del Núcleo de Dominio (Quedan Fuera)**

Quedan **estrictamente fuera del núcleo de Value Objects** del dominio los siguientes conceptos, los cuales deben modelarse en sus capas correspondientes:

* **Agregados y Entidades de Dominio:** Taxpayer (Contribuyente), EcfDocument (Comprobante Fiscal Electrónico Completo), Establecimiento y PuntoEmision.  
* **Servicios de Dominio y Políticas:** TaxCatalog (Catálogo dinámico de impuestos), DgiiPadrónService (Verificación de estado de RNC), TaxExemptionPolicy (Reglas de exención de ITBIS/ISR)5.  
* **Artefactos de Infraestructura y Transporte (DTOs):** XmlDocument / XmlStream del e-CF, DgiiEnvironment (Configuración de endpoint), TaxReturn (Formatos de reporte 606, 607, 608\)2, y DTOs de Request/Response de la API SOAP/REST de la DGII10.

Este catálogo especializado garantiza la representatividad completa del marco tributario de la República Dominicana bajo la Ley 32-232, asegurando inmutabilidad, rendimiento de cero alocaciones para .NET 10 en Native AOT, y protección absoluta contra la mutabilidad de la normativa fiscal.

#### **Fuentes citadas**

> 1. Normas Generales de La DGII A Sept 2022 | PDF \- Scribd, [https://www.scribd.com/document/598554449/Normas-Generales-de-La-DGII-a-Sept-2022](https://www.scribd.com/document/598554449/Normas-Generales-de-La-DGII-a-Sept-2022)  
> 2. NCF y e-CF en República Dominicana: Qué Son y Diferencias \- Digisoft, [https://digisoft.do/blog/ecf-ncf-electronico-republica-dominicana/](https://digisoft.do/blog/ecf-ncf-electronico-republica-dominicana/)  
> 3. Comprobantes Electrónicos (e-CF): guía, requisitos y pasos \- Blog de Alegra, [https://blog.alegra.com/republica-dominicana/comprobantes-electronicos/](https://blog.alegra.com/republica-dominicana/comprobantes-electronicos/)  
> 4. Tipos de e-CF República Dominicana: los 10 comprobantes activos \- Blog Alanube, [https://blog.alanube.co/rd/tipos-e-cf-republica-dominicana-guia-developers/](https://blog.alanube.co/rd/tipos-e-cf-republica-dominicana-guia-developers/)  
> 5. Guía Informativa sobre Comprobantes Fiscales Especiales \- Impuestos Internos, [https://dgii.gov.do/publicacionesOficiales/bibliotecaVirtual/contribuyentes/facturacion/Documents/Comprobantes%20Fiscales/3-Guia-Comprobantes-Fiscales-Especiales-NG-05-19.pdf](https://dgii.gov.do/publicacionesOficiales/bibliotecaVirtual/contribuyentes/facturacion/Documents/Comprobantes%20Fiscales/3-Guia-Comprobantes-Fiscales-Especiales-NG-05-19.pdf)  
> 6. ¿Cómo verifica Alegra la información de los Reportes 606 y 607?, [https://ayuda.alegra.com/dom/informacion-reportes-606-y-607](https://ayuda.alegra.com/dom/informacion-reportes-606-y-607)  
> 7. Estructura y Tipos de Comprobantes \- Impuestos Internos, [http://dgii.gov.do/cicloContribuyente/facturacion/comprobantesFiscales/Paginas/tiposComprobantes.aspx](http://dgii.gov.do/cicloContribuyente/facturacion/comprobantesFiscales/Paginas/tiposComprobantes.aspx)  
> 8. Estructura de las secuencias de comprobantes fiscales electrónicos \- Adm Cloud, [https://knowledge.admcloud.net/estructura-de-las-secuencias-de-comprobantes-fiscales-electr%C3%B3nicos](https://knowledge.admcloud.net/estructura-de-las-secuencias-de-comprobantes-fiscales-electr%C3%B3nicos)  
> 9. Comprobantes Fiscales Electrónicos (e-CF) \- Impuestos Internos, [http://dgii.gov.do/cicloContribuyente/facturacion/comprobantesFiscalesElectronicosE-CF/Paginas/TipoyEstructurae-CF.aspx](http://dgii.gov.do/cicloContribuyente/facturacion/comprobantesFiscalesElectronicosE-CF/Paginas/TipoyEstructurae-CF.aspx)  
> 10. Descripción técnica de facturación electrónica \- WIKI THE FACTORY HKA DOMINICANA, [https://felwiki.thefactoryhka.com.do/lib/exe/fetch.php?media=descripcion-tecnica-de-facturacion-electronica\_nuevo.pdf](https://felwiki.thefactoryhka.com.do/lib/exe/fetch.php?media=descripcion-tecnica-de-facturacion-electronica_nuevo.pdf)  
> 11. API de Facturación Electrónica DGII | PDF | Xml | Ingeniería de software \- Scribd, [https://es.scribd.com/document/636796712/Descripcion-Tecnica-de-Facturacion-Electronica-1](https://es.scribd.com/document/636796712/Descripcion-Tecnica-de-Facturacion-Electronica-1)  
> 12. Firma Digital para e-CF en República Dominicana: Certificados y Proceso, [https://www.apiparafacturar.com/posts/rd-firma-digital-ecf-republica-dominicana](https://www.apiparafacturar.com/posts/rd-firma-digital-ecf-republica-dominicana)  
> 13. Qué son y qué tipos de Comprobantes Fiscales Electrónicos existen en República Dominicana \- GuruSoft, [https://guru-soft.com/es/blog/r-dominicana/que-son-y-que-tipos-de-comprobantes-fiscales-electronicos-existen-en-republica-dominicana/](https://guru-soft.com/es/blog/r-dominicana/que-son-y-que-tipos-de-comprobantes-fiscales-electronicos-existen-en-republica-dominicana/)  
> 14. factura electrónica \- HubSpot, [https://cdn2.hubspot.net/hubfs/2309503/Situaci%C3%B3n%20e-factura%20Internacional.pdf](https://cdn2.hubspot.net/hubfs/2309503/Situaci%C3%B3n%20e-factura%20Internacional.pdf)  
> 15. Documentación sobre e-CF \- Impuestos Internos, [http://dgii.gov.do/cicloContribuyente/facturacion/comprobantesFiscalesElectronicosE-CF/Paginas/documentacionSobreE-CF.aspx](http://dgii.gov.do/cicloContribuyente/facturacion/comprobantesFiscalesElectronicosE-CF/Paginas/documentacionSobreE-CF.aspx)  
> 16. Contribuyente electrónico de la DGII: resolvemos las preguntas más frecuentes \- Voxel, [https://www.voxelgroup.net/blog/contribuyente-electronico-de-la-dgii-resolvemos-las-preguntas-mas-frecuentes/](https://www.voxelgroup.net/blog/contribuyente-electronico-de-la-dgii-resolvemos-las-preguntas-mas-frecuentes/)

[image1]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADYAAAAWCAYAAACL6W/rAAAED0lEQVR4XqVXT4hXVRQ+Dw0Sp6wxHIYGpEGKaBOohTDaxkURYzEKSRPUJg03ilADghDIgAhuFBQislkM/SFoVQQFTegiaOViDAKxhiAKwk1uFLXvO/fed8/9N78Z54Pv9+4799xzz/fuffednwjR6W8TtrtwLQwWvjO9PAAqI5dNKkPsH+RZwo7V9rIh0qwG+7fQGpQq2YyfUV5x2xWTFTGW7eyxEq/V+fgHUdgV62DYIk6HYgj8BpwH3wcfDh0rm9ZjFa4R3nG1Y9XHitTWY+Bp8Otgo7A5MUo96P0ieA68gNtXhU9lAPycNcMG8B1xCdTwKHgUrh/helLKfAjGeBOkzxlEfSbpdXNtD7c1YXT5APwJjadEt6jMdy7gQ8ZPYXVkhmG038D1U/Am+Ac4WviLbAWvgu+K2zGvgL+BLxifTeD34ClxOT8PXgP3ZxmkwrpUGDv/BieMbbxzib1cWZIWhsHXwB3g5+KFaU8cth7tj2H4StsRs7B/J26ViBnwF/Dx4IAQ0/j5Fc2RYJNcmPTCdMZZsU/XJfEIeBm81FsKpC93Bs6BmF2+xcbBv8Ql7qFRpsBb4hKlGIpiDDv7TvA/cF9vob9PGsI6K4xbgYeJJmGE8QEsSPbUVgYN4IUVW3EveE8SYYpJ8D7GTuP6LPivBGERFE3xXIiQZ2vFZKhzAlRYcKJdaO/K7ZQl6lFYe2GZ3QuQmWxEb5cooCXM2pvCeGUCeRJhxXLBpYQ6WsJmECAIsMGssEnnozvL+vTCTA4tYd0InK5LmUS5Yi3UlVaEqeNxiQLsLrDCeEqynQjremGJYCsseceMAL8yboBZMfuexFlSRbat4Bw6Npr0sAnvktmK2hqwFdWnYm+vGI9cHr1ZEr0wnow8IT0KAT2ynjn3sIrVnoD9jpSnYhA81cWT0woggrATmU0YIwqLmXASnkI8jTy6J/CzCB9WIsGVH2vWZ/1Huy2zvmLAk+Dv4ioci8MScxiC6wL8eVqbkk9P1Nv+6lGrPHQe/dkG/gkeDE6w78bPP+CuaJOzsHO7fGhsPkRRuXMOxhzLxPOW1cTP4qoLmvigvgQ/k/jRfgtcElcJqZOYcWauirAU/EDeAA+Bb4srX45IuijHxH2DfhD3rpoubXM1uXX5EeUDIO+KE/iecaWgb8EvxB0Un4BXwLHex+2Ki+CP4OviRC2KK63MzGHFiiI4qR42i9vrJNsV6BY9D27Uu7SzR8tuwAKbSR3wydUKboZ5WtRH9kilbpX2ivkU0ksdrpNbkwXzYFRiFvErPi00fGxJVRTBKwXHcts8524bU60JrZgtexBWrNgq0OmJ9VJuDohTN5NYO8rQ/Vbku8G/FDxxeNV3pUQZoUTbp90TMMCj1R3t4aBa+h/swrSdLHWQdwAAAABJRU5ErkJggg==>

[image2]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAFwAAAAXCAYAAACGcCj3AAAF+ElEQVR4XrWZXaimUxTH1y5ECBFNPpo0N0hMDDWoUVzMxbhwYUw+bqRxRU3JxwWnJE1ESrkwcg6JEVcoJJ3GhQsXXHCjptCgiKkJhTjWb6+9n2d/Pc/7nPc986//eZ9nr4+999prr73f94g4McTPCRhVTYSjehuBDetgPkduPrMc5mMjPAWMuZpjsUvMNnWzlYK8VqtbPHykB2Trw7CTTNJSa7UthAUczhysWNAS0YBWj4ZCoylgWFJhQHWT8lHlmaUgRcP2BOX9yitLQYRty8oyt4viTq3ST1tuV+5sqIyZT8Jks0mKg0o+JE8ojygvKIQJmg5uUP6lvDVtdG3dBK5pNwFblD+KJccQ9ih/V64Fvq88OdOIsGHeJ52u+1s/l8X0tyt/6mXytfIcb1HBO7pZ+W+jvWq6Sj/+COQ5E7bhBacpV5Vr+vZQJk7QcNHZyYhdiuCDXXFAzG4lEbdAwA4qf1F+qjw9FxucLeCHyn+UTxbiiGeUP8h4Qp6tfEN5rBSUEThF+Zrye7FOr6exESSPop3M+EIsM4rBmmbtx7ckdjbJTK82iiCDvhIb59tiCxCAUWZ4vvJV5TvK78RKZokTxXbKU2Jj2ZmLBZckB/F5XloJaaBj5vSI8r9CVoEt/bTaPCwWuF2lwgAuFJsMg2QgK8NxylDZ5eLBYsTED6qQoBPA1dBmcNU6kTj7ne0gH6iGX8bA/F8RKx0XZ1IzoO0l5W1iixLiky3wVrGAE0OSYRDnik2eIDAwHN7RSb2/xjCtURfJDzaWoyLjeiQe1mVXgAk9KJapBPxz5VmZRg7mQz/QsjcMJHww9yWxnUBt/lh5aj/W7ukWsczdpk1/avu+TgU4XyGWxPxwVhzO5AWYwJ3hmZWbWlMJFqvOlowBWJU049oYscsypgQJ8brKqZPorkpZJnJTFvBF5SXKm8S2eZ9Ipv2A8jLpF75ZEsWCiQ98/SqVnuwWkxPwb8USCISt2g9MO3PUphAkX0q6mlqht2NFl8W2EagyLqoWIZxpV0YtvDPw/WKZBuKBW5eAHrF+c1BeoTzm0luNk2v0773hba8M1W/rixhxUJ4nlr268N1AN4slKAscFzbfAUFXM8y9oI9czaI5NY/6sxI1M/SxYEUfS1oIFkEjeJvKmCWvu120s8bczqMMuMe1wq5wjl0BmBxZNHSAAV+/w3Nc2DgvgrikPEOsw3b9Fi+M9ZsbT1xoyDPjINibw6h57i4dhn4+HDzcF+P9MuXwndXqHreE0gay3dh2Vdwcdi7auc7GlXY12BXvSt3Xmvojm8iqgKzTWL8BWc610Ndo5d3SB4U7dVe/Q1uKWL8BHZDdZDnZTt8kHyBesX4jy8DKcl/cUrSP1GI/GTKTUzjW/BRkz1DGYTxq57xdsUqGcjdFxAO+daNK63d8Z0doYN0OsdIS/W1T6kFIGa2zRPr6HUG5JUF2iO0gEgJQco5IegFI3FFjqFuGXhADfljbqlUSq73L0neSYkVtCEBdB11hl8+LgLftbBJvie2qEjHg+8o4idVv7NJz4YDyZ7H+8Bsxq36TmGmpIXYsEIHdmvRt9dvl9RsxW+lL5UWpIIDM/0zK09/ApD8Ru4u28Li0M66zo/M6Ntj5ElPasb1fNnkT8U5M4FO/PO5xVpfT7tCjBMUSAGLmN+u34jrle5J/Q90VEosyE/z7DzJf67fr6jeH41HpayCdXBqFimeV/IYQ5TwzYQJ/yGX10+1NZsLK/tbLPFnQq5WHgn6s1f2uGrbjmsatIG1/LtiAy5XfFHK+IW/Xed8l+Rz4in1jsCNQZD0HL4v5ZqHL7y7M9ySpY6V67h6ciJXMj8S+xgPilv5mg88PitRq5NkYJqlPUmqiZZm3tTTa8JrN/xj0bePSAXQKMzXHsaB5huY8N7SHBTHPUOaxmWw0pjcmayBXT94m+ZmklGGqxbr1Bg0GBREzFXKsU73DvHaLgn4jK8TGprDAFJ3jhvUMdA4cJ7eLYGxIY7JhFFb22nZV/kja1qoxRW+KzkZgrJ96diPaI6IW/gdD3D37TrTrRQAAAABJRU5ErkJggg==>

[image3]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA4AAAAWCAYAAADwza0nAAABWElEQVR4Xn1TO05DQQxcK0qBIqVBAqWMQEKiokufCzyJioor5AK5AeICSDlBDkBq0lBxGE7A7Piz3rwnnIzGHnvt/SSluEl4TQpN+LEv45FNSM1q8r/F1XKyK2Fgq3Mi63WvkRutjtGVzaOepqUGEn7XKdsosQT2wBdwR6V2Ubtk2jXwBu0MdYt5syjKZSlawz8grBM27USpmhPboz0AR4RH8GNNa5FlyVHLVRvwJ/gArDXZbTk3Dxug/SLxrHvVDbdR/a2yAWXtxEuAdwZvi1+C1fRGJcv0l6A98A0MiP0Wm012sgcGLYAd3B/Er+CrUe1k3PQ5urwUbbADFpbTvwynWKWeWewSo8MM3oDwBP/eK5W6umzip+ik4KanH7I39Scw2YM5nFvwqkLIskItYyJ8uSnppp+AjwohC/2AuEb9vTbVTdno2InH6eyXnq3RG+yO0JK9ZvYHT74aPb3135kAAAAASUVORK5CYII=>

[image4]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA4AAAAWCAYAAADwza0nAAABTklEQVR4XoWUO0qEMRDHMywismAjKNtbi51gp7Vgu/V6AC8gXkDsrLzFnsDCZguRPYQH8TdJJpnM94lD5vWfRyazjyQiKXGUxAyzNNYwo54TyAHetCahd24+9ggdc53HcsHMaMMT6gUu7Kiirmu0/A5M3yB3uGv0QUVbMPoDgnOE2mDs0Q/w0jLsvkZ1uObVKfXGNe4XwBP28Uxp8GRwF3i36E9avqBP/q70I/QNq7hCfKDf4FXLnaNWXyX6EPGM9YM+D4k13VWRyNJkA+9xHuHlWKDHX68Jmpjkm4D7mKx5qemAblGSbnMH3yddUg6pcEXVUqGP1sfrEq4ZcdGTWndPGbmDt/BFCl+MThPYgFjhRpuQjTwk2C/F3B5reIFEj27sDF5JeWvRUuzOovqUXuX90CXgO/p/lvSaSlO7Pbxn8pfQZnfK5YTyTD3WrV8sDRoRnYWrGwAAAABJRU5ErkJggg==>

[image5]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABwAAAAWCAYAAADTlvzyAAABfklEQVR4XqVUO05EMQy0O3oqOqCho+UMnAzRgygQJ6BFoqBYCXEw7DiJvwlPYiQnznhiJ37ZBTgKnMOcBnQZAiUWGk+jq4Vpk7B2tYVLUqk3IcBcPhH9PI6Omp0gaQ1mLOjzsVAIN0bNCkmnJVKoghftt9horSxZPKOBjX0XiWjRLnEd2LXDbuq4pvUXzY9En8eWzFxY5HWfwtBhXQB56y05n2RvZFdCd0vg6pEL2MZtEOGG7L0ZkB/CGRrd6/4ANZVvybf9JrtrVFAoMxwjWbR7D1FfkD3DKNw+7yqZPYTh5hiQyc4gPyZ4odUPzZeNyuKEJpkpIiyjPt/uiexEBYq2ZqwFKaIEyit9pfkD5PXawzpUHGPylUBvjZQc/U+jgm4A6W3+t1TkCDPcrhNI+7iN/0AoIOdRmuZ7mh6g/csoXEv47iY2aHHsdV3eCZer8i0O84lQlAUds3jzNduxySpw/Cjkl8ewE2M/e9DIErtjg8Yf4VV8rM2h3eel4RcPPhHyExe5nQAAAABJRU5ErkJggg==>

[image6]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA4AAAAWCAYAAADwza0nAAABWklEQVR4XoVTu0oDURC9A7GwEwXBwi5d7CyChSCERbAO+C+inY1CioCCPyL4PbHIB/gDnjvvyQYdmJ3HmXtmdvZuowYhfv4h1P6rSNJLTSWOSNpFmdMmOJeXtol0NDHlLix3iE4V66C+Q2lLcjBOTeF/wB4K5vkQW0UeBvoIHdIBg1VzV27K/hQq3QyPRSf+RKvBC8xQoVyfUklmIH+DPShZKYoFyWQ+Rw9f8bi24rRnOob5grNBvGHrSlvkfmC/oZyjrtSehNfEGdmdwKyxnKs6et75HgHrJaAVnEkM7pjbEyTP4LIicd5k/becZ8xw9o/6OWyLFnCWofQA+wm9B+2SNI/OVjPXvjYv2/5u73Avdl+jVNXxmQSrp2e5pXE9RpIB4o/MH3umJAUMojHdQHKZ5YpqcUgZzyzhd+FN9gtdkP0x6VDUbuD0bfpApWjXlZF8roSlW1lQkV8JPR2PVgcxHQAAAABJRU5ErkJggg==>

[image7]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAC8AAAAWCAYAAABQUsXJAAAER0lEQVR4XrVWTYiWVRQ+B2fAATVCaRodiUITadPwUSKoKxczSBI1i3SCQNCgjThCE0EQDC5y4UJQI8RmiOgHsU1FP4smp0XSRhelCGIjwmBQq1pY0PQ899z73p/vvt80C5+P573fOffce5/33L9XpAJ15DP3lWUeEbyhTGq1y/O/UGqIKNV0G61NV45ePZV1pR3Q5vdYpjpHGlxtGJ216prPYw34CLiqR2JXgWtTxwpgHWl7563wLate4AX8n0f5JngJfDSrNVD4G+Bk4tMBPA6C74EnwW1JpUPeSWatg3lUrO1b4JCr7m6wAzwNngX3iQkJ2AxeBXd5+2XwD/BtcCtaD6EcBb9BR1+LzZDDQ+C34LR3Pg3+Ar7IyjJPheMx8Bp4GFwNjqH6JspnQ4Bai9fx+B7l4+B68EOxl+33YWPgAjjs7SfAU2Lx4+AE+LzYjIz4GGHXU3j+BD6cOCfgv44/g9HHUC/F0AeeBy+q/Q84AfsrxHE2iQ54T11Wm8YUR7Gj3kcNC+h/yEcw07OSZBh4VfLl4gRTOANTPAP+iY72mxkGjcrFBCyKDZwCa1f+EhNNnBATSkEeyg03j97epyHWJo0ZhvddsdkkngI/kPxlZDv4uwTxUSMGVgrgwBm4UX3YXvBfWFP5O8lz4JLYVGNw/UKYVa7bGEcRcxJnnHvsBjpn0giu72P+P2dwRtLl4vvpqDqRs+Yxr1rWnN95cnEOGkW6zCchqT+ITLNKlH42Pw5eBg+Bn0tc/1wqzXJp9CQCZkOlRweVyUsFZG9BcY34BOhTzW+nBAXm4rVLfMAWMU3rvM1sfyx8WRuaM8TT8ABt7vJEfCOuoz7z5s1EBzAbhXgXZ+JxEKht+FvSLXKN1sU7+NH4ghQelgtPsCve5ukWM8wGicRm2USXZAHazJqa+HhrVZaNmsjY3vm1RbwHkqOTvg1Ps4+02YPuXsKJobKoqUgLDuJ52xVVDXih/CPVZSNLaicIB+VRuoDG4RgENCwb3qhdVz3iRjQsF8MgnJzBbCyupTmxEyEcS8ReNP4b2eSJ4qB2mW3U+A6bwF/Fbk2LsCfPY55gPMkIDmh2VL8B/FmathnK5UKEvcOlaPAzzav4jtht5tz4TYM/ihPsgngr8ia9D3Nn1CnT2sQ58Mb8VGyK+/wA3IR3wZeidtmN/7+h3Ekj8ROTPgEOvs7dCxIz793qBjwH6ztxV7DyM4FZ4WdCALPxpdjVz08C8e0pmv5PxDb/BfAHsUumgdoSug0eAV8R+/x4zaoycLnMoBwoK8SEfwb2qyWkAbP9pNh3xB4GpJVdyHvmBxb3yLhaGT+4/NTaaaWcPe4Hkv9LcNmeCcIq4rlJ31GbAX6LSYxyZdGk0kPF1RtNg7SlZmbZZ2m3I/8wp6OwHwRK5Xy0j1utqSalZi+HXmOn/l5xRUVrWImWG7QHKvEVVxdqMYUvM71Re+f/AIcXqos3E9jGAAAAAElFTkSuQmCC>

[image8]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEIAAAAWCAYAAAB0S0oJAAAEeElEQVR4Xq1XTchWRRQ+BxOUypBA/UChQosIwQ+1hYirkFwUoi3UXAURRJs+QcWVIC500SIQxZUS0beyjS7UKMmN5kaFVIgQJZAWtcpNIPU8Z+bO351733nVB847956fZ86c+bnzijwrtFTkmGCW3EOTt6HIIf2YpYYm79ypF5Ln3oyhsHTwkwvxbKjza0t33mOy4wSfUeMUqJewCpWX8LsMsqA0BajZXi7VHlN0NgWaWXvGnqIFOxB2Fe0hyDnIigoPi3AQMudec/trkF2ZJsdbkOOQ05A9kMUkMIrIs9jb6HMcesa0wvNrwt+D5zcf5lLyr0IqN9Fu9u97kcPfaA9DvwbtDGLfR3sJchHClWN4G/I55EfIE8jZzpBDd+LnDmSduOAjkMuQVxInPl9Gx7TRh76MYWwFvnqu2alVfk347Zl9jvFvA88DtCs98RuQryCvQz6CfAz9dnErZTZEiSvEdoRsQvuHFIXwqa7C029Ckjj9SyE38PxF5wsccDqzda7oWO5Clke3HsAvjj/C88sgv6HPTx8WYsa/s+WYwswDn0nYEn3MgJQEWSE82NljyPpEx3F+C7kirhNLXEO8L6HIRsg/kA87RQrvVefXMf6Agl93SCiEbVusDDkFWeT934F8I+QMKeZg5YpCBM+vJUs06On7SNzy48r6y+tSMOYxIo4WeoMxKfm1LARR8OsAv6b8PDPuiSsQwfPgS//M8+WMFFuiRFgRPrkU7LyfqFIfBmADloFEK/oI46nw5/2O8Gd6Zr4P8jPkE8h5cauCmIPVtkQ+vByVFWHgsrwikxP9APKf16WIifYqbM+BXxN+71Xw6zi/h49dLS6nJV49C/28uPsF0X0Bd/v3gPyMiPm+CPlBikKkifoBbJNaIbSfaISx5PymCp1Hfn0a/gAOfl7jlngXcl3c+6fByyOsiFiDgLPJFij0Uy/dGuKAK3pJ+LXPU6y4Krgd5rz5Bch3Es4U7d1VKlsjMDMombEA+vKTyz3IA+3RSKK85Qk5K/lG/hTu7Mj4JfAHlh5/brZZn5f4+eRn9nfhZ7aSCFEpRAA/TU8Q9557NQZ+ji544bPtdY3vXT+I0X+tNYX9MJlXndkAfuVlLvVZhMb4NeHnu9niKJiT4zdko7MtAdVsUiB+VjlO3jdK2Cx1heC3O2MTl/QvUB5OLDyMMFuaXsn3Qh6Ku8UR9OYt8Jrw1ulieUj9KW6mO7+n4tfAr9c0v+F6KLcEL0+Jin+ylP9DukKEHllJJsUZwWFkJzMvKLcha5OKbIDch+wXd1XlDe8YZGF0UT6fhPwE4TWWRfhV3FW4A5f5LXEHZJp8A789V/nTmfPP3BJnpPy/4owswvfi+FjwiJRoBDzht4q7kvNa3IPNksqb4gazRayznN3e6h0GfhngFxdZ8PfouK1OSDlIwjmyOCwyVwb/u0REonqGbagMuIIhvcEbR32mRTtZ6jkhaoK5h5Hpf354jvyjVC3GFp9RtPi0oXlaR40y2Z6juVuPYf+W6GZUuhni/x/TDu3aI2oVwAAAAABJRU5ErkJggg==>

[image9]: <data:image/png;base64,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>

[image10]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACoAAAAZCAYAAABHLbxYAAADI0lEQVR4XqVWz6tOQRh+J+5ChCJS5He6FpcSNpQk7K6UUiSxsLxhIWJhIQs/isR1YyELioWN/FxdG1ncUv4BUkKom2vnx/PO+57vzDnvvOd83/XU882c931m5pk5M/MdIkaIv4JYrwbSp0aYthk0pr1kSFOeqG57EjCNNGDiChOvBfKGTKveYXx5fRqhLxVUlUbbHmhF915a8F+NBdzFRvAqeB3cA04vUx0sAM8gNILyLLgiTfrwHc4ET4Oj4PJaro6p4BXwHLgQXAmOout3KBezQIfZAL4AN4NrwMfgXySPs6S04ptKU3PAC+ArcCs4pUy56EcH31A+Rz8zNLYvsAlZYcY08BF4iDp9Bh7rDTiB+jrVWdR8LwVvk6wgv8KGaRmsAj+Db4knKi13kxi9pRp+5e/B8SCrSTrEKdUdU50LHuSBcjX1ZjDFbCpXk/u4Bv4hMczoo7g9wjNi0+UoJ0iMcmkAWeBVe0KyiryaadJHmswLOboD/A5exFNfVigh3tsPwd8IbEnTRZNBcJxktpleclBZtahjG/gB/ALeBOdW0wa8WBMUb4DQF/ssfpKHyqEJvMGd0S1ahXxYzoM/SMwn43YwC3wJ3qV4jaXJitFOvbiGXqPOK93NKRdk/Cah9eAvildUmFeGo4b36zB4meQ26Al8OQ+BY+joAHkdZMwBA4jzNTSQpItTXr96+BUPQ3+SykXpB7cnGg+V0Xm2e8ExcCh0/l0yKJvdITm5XBZgczAZ71c2wuAWfLkf1XqBI1TeDt1C24c420E8PaXGv7mo56vlK7gzSewnMX8DGj7dLDyIgrfDR5JDV5BviE3arhn5N6poTEbwwbhHcjgOk6zaT/C+5hjFVmDzylDUP4HLVJegceBqslGqiBr56FoC7hKGRammaxQDasn7cD7JLNvIJ7b9JlC3tYAPTTerAq3FL39mZRiklM+wEXR0icRwVygH9ix4cYVNVz+QTd4EFJmV8KQRmWQMeR1k9AIv4cU9eHrPldF7ccrETCAbqiEzgJprb1uF1Xuz7AH5FploJhSRmV8lYOyZBt5KZPa0EZrApPEPC/VnZgGAvecAAAAASUVORK5CYII=>

[image11]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABUAAAAYCAYAAAAVibZIAAABIklEQVR4XpWSvRJBMRCFb2YUCowZlWF4CB5GpVSovIRSZdQKpZe0kc3Nzzl7wzeTbHL2J7uXrvO47x4NnLlg4TQ2bb1jsAY4K4HUA4iUGHRGyAM8j6tJt5JhjByiY3wZBA2TGhzXiLUmUOBhpAxJNwgfi3QSO8nFgfgK4tfhrmJ2UWg0ETEdEV/wJlGjKDQzPFaQ6r7hi9hjaLKK1u7nsi/bywXrvveN5D7F3mXNfJkAvvEPe1lvKbD2l6KOdprOAHUuZL1kbTGJfQogV/vzWdYhcwSim3dKyifJ/08fYlckKmA6eiDC/95T5rDR1qHrfKRcbwGF9PxjegVkkY6IBES9HUs61U/BJyuuZOTBF8kEhQAOxcpTLD1geYnOJ8sl130ATAAJQoqukIMAAAAASUVORK5CYII=>