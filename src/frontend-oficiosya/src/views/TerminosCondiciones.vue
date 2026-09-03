<template>
  <div class="terminos-condiciones">
    <section class="section legales-section">
      <div class="container legales-container">
        <div class="card legales-card">
          <h1 class="legales-titulo">Términos y Condiciones de Uso</h1>
          <div class="legales-fecha">
            <p>Última actualización: {{ fechaActualizacion }}</p>
          </div>

          <div class="content">
            <h2>1. Aceptación de los Términos</h2>
            <p>
              Los presentes Términos y Condiciones de Uso (“Términos”) regulan el acceso y utilización de la plataforma
              digital TuOficioPro (“TuOficioPro”, “la Plataforma” o “el Sitio”), destinada a facilitar el contacto entre
              personas que requieren servicios (“Clientes” o “Usuarios”) y personas o empresas que ofrecen servicios
              profesionales u oficios (“Profesionales”).
            </p>
            <p>
              El registro de una cuenta, la contratación de servicios ofrecidos directamente por TuOficioPro o la
              utilización de funcionalidades que requieran expresamente la aceptación de estos Términos implicará la
              aceptación de los mismos.
            </p>
            <p>
              La mera navegación por el Sitio, cuando no requiera registro ni aceptación expresa, no implicará por sí
              sola la celebración de un contrato ni la aceptación de obligaciones económicas.
            </p>
            <p>
              TuOficioPro podrá modificar estos Términos cuando resulte necesario. Las modificaciones serán publicadas en
              la Plataforma indicando su fecha de vigencia. Cuando los cambios sean sustanciales, TuOficioPro podrá
              comunicarlos mediante correo electrónico, notificación dentro de la Plataforma u otro medio razonable.
            </p>

            <h2>2. Identificación de TuOficioPro</h2>
            <p>
              TuOficioPro es una plataforma digital destinada a facilitar la búsqueda, publicación, comunicación y
              contacto entre Clientes y Profesionales. Los datos de identificación, domicilio legal, medios de contacto
              y demás información correspondiente al titular o responsable de la Plataforma serán informados en los
              lugares correspondientes del Sitio.
            </p>
            <p>Para consultas relacionadas con estos Términos:</p>
            <ul>
              <li>Email: <strong>{{ contactEmail }}</strong></li>
              <li>Ubicación: San Miguel de Tucumán, Tucumán, Argentina</li>
            </ul>

            <h2>3. Naturaleza y función de la Plataforma</h2>
            <p>TuOficioPro funciona como una plataforma de intermediación y contacto digital. Su función principal consiste en facilitar que los Clientes puedan encontrar Profesionales y que estos puedan ofrecer sus servicios.</p>
            <p>Salvo que TuOficioPro indique expresamente lo contrario para una determinada funcionalidad:</p>
            <ul>
              <li>TuOficioPro no es parte del contrato celebrado entre Cliente y Profesional.</li>
              <li>TuOficioPro no contrata a los Profesionales para realizar los servicios publicados.</li>
              <li>Los Profesionales actúan de manera independiente.</li>
              <li>TuOficioPro no establece el precio, alcance, duración ni condiciones particulares de los servicios contratados entre Cliente y Profesional.</li>
              <li>TuOficioPro no garantiza que un Profesional acepte realizar un determinado trabajo.</li>
              <li>TuOficioPro no garantiza que un Cliente contrate efectivamente a un Profesional.</li>
              <li>Los acuerdos relativos al servicio, precio, forma de pago, materiales, plazos, garantías y demás condiciones son establecidos directamente entre Cliente y Profesional.</li>
              <li>El hecho de que un Profesional figure en TuOficioPro no implica que TuOficioPro recomiende, avale, garantice o certifique su capacidad, experiencia, matrícula, habilitación o calidad profesional, salvo que se indique expresamente lo contrario.</li>
            </ul>

            <h2>4. Relación entre Cliente y Profesional</h2>
            <p>La contratación de un servicio se realiza exclusivamente entre el Cliente y el Profesional. El Cliente y el Profesional son responsables de acordar previamente, según corresponda:</p>
            <ul>
              <li>alcance del trabajo;</li>
              <li>precio;</li>
              <li>forma y momento de pago;</li>
              <li>materiales;</li>
              <li>plazos;</li>
              <li>garantías;</li>
              <li>condiciones de cancelación;</li>
              <li>lugar de prestación;</li>
              <li>obligaciones de cada parte;</li>
              <li>cualquier otra condición relacionada con el servicio.</li>
            </ul>
            <p>
              TuOficioPro no participa de dicho contrato, salvo cuando expresamente se indique que determinada funcionalidad
              de la Plataforma implica la participación de TuOficioPro. Los conflictos derivados de la prestación del servicio
              deberán ser inicialmente tratados entre Cliente y Profesional, sin perjuicio de los derechos que pudieran
              corresponder conforme a la legislación aplicable.
            </p>

            <h2>5. Registro y creación de cuentas</h2>
            <p>Para acceder a determinadas funcionalidades, el Usuario podrá necesitar crear una cuenta. El Usuario se compromete a proporcionar información verdadera, completa y actualizada.</p>
            <p>El Usuario será responsable de:</p>
            <ul>
              <li>mantener la confidencialidad de sus credenciales;</li>
              <li>evitar el acceso no autorizado a su cuenta;</li>
              <li>informar a TuOficioPro cualquier uso no autorizado;</li>
              <li>mantener actualizados sus datos.</li>
            </ul>
            <p>
              No está permitido crear cuentas utilizando datos falsos, suplantar a terceros o crear cuentas destinadas a
              cometer fraude, engaño o abuso. TuOficioPro podrá solicitar mecanismos razonables de verificación de identidad
              cuando resulte necesario para proteger la seguridad de la Plataforma y de sus Usuarios.
            </p>

            <h2>6. Edad y capacidad</h2>
            <p>
              La Plataforma está destinada principalmente a personas mayores de 18 años. Los menores de edad podrán utilizar
              determinados contenidos o funcionalidades únicamente cuando ello sea legalmente permitido y bajo la
              responsabilidad y autorización de sus padres, tutores o representantes legales. TuOficioPro podrá restringir o
              cancelar cuentas cuando detecte que la información proporcionada sobre la edad o identidad es falsa.
            </p>

            <h2>7. Obligaciones de los Profesionales</h2>
            <p>Los Profesionales que utilicen TuOficioPro se comprometen a:</p>
            <ul>
              <li>proporcionar información verdadera y actualizada;</li>
              <li>informar correctamente su experiencia y servicios;</li>
              <li>cumplir con las habilitaciones, matrículas, permisos y requisitos legales que correspondan a su actividad;</li>
              <li>cumplir con sus obligaciones fiscales, laborales y previsionales;</li>
              <li>emitir comprobantes cuando corresponda;</li>
              <li>contar con seguros, permisos o habilitaciones cuando sean legalmente exigibles;</li>
              <li>respetar los acuerdos celebrados con los Clientes;</li>
              <li>no utilizar información de los Usuarios para fines ilícitos;</li>
              <li>no publicar información falsa o engañosa;</li>
              <li>no suplantar a otras personas o empresas.</li>
            </ul>
            <p>
              Cuando una actividad profesional requiera matrícula, título, habilitación o autorización especial, será
              responsabilidad exclusiva del Profesional contar con ella y mantenerla vigente.
            </p>

            <h2>8. Profesionales matriculados y actividades reguladas</h2>
            <p>
              Determinadas actividades ofrecidas mediante TuOficioPro pueden estar sujetas a requisitos profesionales,
              matrículas, habilitaciones o regulaciones específicas. La aparición de un Profesional en la Plataforma no
              implica que TuOficioPro haya verificado la totalidad de dichos requisitos.
            </p>
            <p>
              Cuando TuOficioPro realice una verificación específica, esta podrá ser identificada mediante una etiqueta o
              distintivo indicando qué información fue verificada. Por ejemplo, una identificación como “Identidad
              verificada” no deberá interpretarse como una certificación de conocimientos, experiencia, matrícula o calidad
              del servicio. El Profesional será exclusivamente responsable de mantener vigentes las autorizaciones
              necesarias para desarrollar su actividad.
            </p>

            <h2>9. Obligaciones de los Clientes</h2>
            <p>Los Clientes se comprometen a:</p>
            <ul>
              <li>proporcionar información correcta sobre el servicio requerido;</li>
              <li>brindar información suficiente para que el Profesional pueda evaluar el trabajo;</li>
              <li>respetar los acuerdos celebrados;</li>
              <li>pagar los servicios contratados conforme a lo acordado con el Profesional;</li>
              <li>no utilizar la Plataforma para actividades ilícitas o fraudulentas;</li>
              <li>no publicar información falsa;</li>
              <li>no acosar, amenazar o discriminar a Profesionales;</li>
              <li>no utilizar información obtenida mediante TuOficioPro para fines ilícitos.</li>
            </ul>

            <h2>10. Publicaciones y contenido generado por Usuarios</h2>
            <p>
              Los Usuarios podrán publicar fotografías, textos, opiniones, reseñas, comentarios y otro contenido permitido
              por la Plataforma. El Usuario que publique contenido será responsable de que dicho contenido:
            </p>
            <ul>
              <li>sea verdadero o represente honestamente su opinión;</li>
              <li>no infrinja derechos de terceros;</li>
              <li>no contenga datos personales de terceros sin autorización;</li>
              <li>no incluya contenido ilegal;</li>
              <li>no contenga amenazas;</li>
              <li>no sea discriminatorio;</li>
              <li>no sea utilizado para realizar fraude;</li>
              <li>no infrinja derechos de propiedad intelectual.</li>
            </ul>
            <p>
              El Usuario conserva sus derechos sobre el contenido que publique, pero concede a TuOficioPro una licencia no
              exclusiva, gratuita y limitada al funcionamiento, promoción y mejora de la Plataforma para alojar, reproducir
              y mostrar dicho contenido dentro de los servicios de TuOficioPro.
            </p>

            <h2>11. Reseñas y calificaciones</h2>
            <p>Las reseñas tienen como finalidad permitir que los Usuarios compartan sus experiencias. TuOficioPro no garantiza la veracidad de todas las reseñas publicadas.</p>
            <p>No se permitirá:</p>
            <ul>
              <li>publicar reseñas falsas;</li>
              <li>publicar reseñas a cambio de pagos o beneficios cuando ello implique manipulación;</li>
              <li>amenazar con publicar una reseña negativa;</li>
              <li>crear múltiples cuentas para manipular calificaciones;</li>
              <li>publicar información privada de terceros;</li>
              <li>utilizar reseñas para extorsionar, discriminar o perjudicar ilegítimamente a otra persona.</li>
            </ul>
            <p>
              TuOficioPro podrá retirar, ocultar o limitar la visibilidad de una reseña cuando considere razonablemente que
              infringe estos Términos o la legislación aplicable. La eliminación de una reseña no deberá basarse
              exclusivamente en que la opinión sea negativa.
            </p>

            <h2>12. Sistema de reportes y denuncias</h2>
            <p>TuOficioPro podrá poner a disposición de los Usuarios mecanismos para reportar:</p>
            <ul>
              <li>Profesionales;</li>
              <li>Clientes;</li>
              <li>publicaciones;</li>
              <li>reseñas;</li>
              <li>fotografías;</li>
              <li>comportamientos abusivos;</li>
              <li>posibles fraudes;</li>
              <li>actividades ilegales.</li>
            </ul>
            <p>
              TuOficioPro podrá analizar los reportes y solicitar información adicional a las partes involucradas. Según la
              gravedad y las circunstancias, TuOficioPro podrá advertir al Usuario, ocultar temporalmente contenido,
              suspender una cuenta, limitar determinadas funcionalidades, eliminar contenido o cancelar una cuenta. La
              intervención de TuOficioPro en un reporte no implica que la Plataforma asuma el rol de juez o árbitro de la
              relación contractual entre Cliente y Profesional.
            </p>

            <h2>13. Conductas prohibidas</h2>
            <p>Está prohibido utilizar TuOficioPro para:</p>
            <ul>
              <li>cometer delitos;</li>
              <li>realizar estafas o fraudes;</li>
              <li>suplantar identidades;</li>
              <li>publicar información falsa;</li>
              <li>distribuir malware o código malicioso;</li>
              <li>intentar vulnerar la seguridad de la Plataforma;</li>
              <li>obtener información de otros Usuarios mediante engaños;</li>
              <li>acosar, amenazar o discriminar;</li>
              <li>publicar contenido ilegal;</li>
              <li>infringir derechos de propiedad intelectual;</li>
              <li>manipular artificialmente las reseñas;</li>
              <li>crear cuentas falsas;</li>
              <li>utilizar automatizaciones que puedan afectar el funcionamiento de la Plataforma sin autorización.</li>
            </ul>
            <p>TuOficioPro podrá suspender o cancelar las cuentas involucradas en estas conductas.</p>

            <h2>14. Contacto entre Usuarios</h2>
            <p>
              TuOficioPro podrá facilitar herramientas para que Clientes y Profesionales puedan comunicarse. Una vez que las
              partes hayan obtenido información de contacto, podrán continuar su comunicación por otros medios, salvo que
              una funcionalidad o condición comercial específica de TuOficioPro establezca expresamente lo contrario.
            </p>
            <p>
              Cuando TuOficioPro cobre exclusivamente por suscripciones, publicaciones, publicidad o funcionalidades premium,
              el hecho de que Cliente y Profesional continúen la relación fuera de la Plataforma no generará por sí mismo
              una obligación de pago adicional a TuOficioPro, salvo que las condiciones comerciales contratadas indiquen
              expresamente lo contrario.
            </p>

            <h2>15. Suscripciones, planes y servicios premium</h2>
            <p>
              TuOficioPro podrá ofrecer a los Profesionales planes pagos, suscripciones, publicidad, posicionamiento destacado
              u otras funcionalidades premium. Antes de contratar un servicio pago, se informará de manera clara:
            </p>
            <ul>
              <li>precio;</li>
              <li>moneda (pesos argentinos, ARS);</li>
              <li>impuestos, cuando correspondan;</li>
              <li>duración (mensual, trimestral, semestral o anual);</li>
              <li>condiciones de renovación (manual por transferencia bancaria, salvo que se indique lo contrario);</li>
              <li>funcionalidades incluidas en cada plan;</li>
              <li>condiciones de cancelación;</li>
              <li>condiciones de promociones (por ejemplo, períodos gratuitos de lanzamiento).</li>
            </ul>
            <p>
              La contratación de un plan pago estará sujeta a las condiciones informadas al momento de contratar en la
              página de <RouterLink to="/planes">Planes</RouterLink> y durante el registro profesional. Cuando corresponda
              legalmente, el Usuario podrá ejercer los derechos de arrepentimiento y cancelación establecidos por la
              normativa vigente mediante el
              <RouterLink to="/arrepentimiento-y-baja">formulario de arrepentimiento y baja</RouterLink>.
            </p>

            <h2>16. Pagos realizados dentro de TuOficioPro</h2>
            <p>
              Cuando TuOficioPro cobre directamente por suscripciones o funcionalidades propias, el pago estará sujeto a las
              condiciones informadas durante el proceso de contratación. Actualmente los planes de pago se abonan por
              transferencia bancaria (MISMO / CVU o alias), quedando la suscripción pendiente hasta la acreditación del pago.
            </p>
            <p>
              Los pagos correspondientes al servicio prestado por un Profesional al Cliente serán realizados directamente
              entre ellos, salvo que TuOficioPro habilite expresamente un sistema de cobro o procesamiento de pagos. Cuando
              TuOficioPro participe como procesador o intermediario de pagos, se informarán las condiciones particulares
              aplicables a dicha funcionalidad.
            </p>

            <h2>17. Derecho de arrepentimiento y baja</h2>
            <p>
              Cuando corresponda conforme a la legislación argentina, los consumidores podrán ejercer el derecho de
              arrepentimiento respecto de contrataciones realizadas a distancia.
            </p>
            <p>
              TuOficioPro dispone de un mecanismo visible y accesible en
              <RouterLink to="/arrepentimiento-y-baja">Arrepentimiento y baja de servicio</RouterLink>, conforme a la
              normativa vigente. Asimismo, cuando corresponda respecto de servicios contratados a distancia, el Usuario
              podrá solicitar la baja del servicio por el mismo medio.
            </p>
            <p>
              El ejercicio de estos derechos se realizará conforme a los requisitos, plazos y excepciones establecidos por
              la legislación aplicable, contactando a <strong>{{ contactEmail }}</strong>.
            </p>

            <h2>18. Cancelación de suscripciones</h2>
            <p>
              El Usuario podrá cancelar una suscripción conforme al mecanismo habilitado por TuOficioPro en
              <RouterLink to="/arrepentimiento-y-baja">Arrepentimiento y baja de servicio</RouterLink> o por email a
              <strong>{{ contactEmail }}</strong>.
            </p>
            <p>
              La cancelación impedirá las renovaciones futuras, sin perjuicio de las obligaciones que hubieran sido
              válidamente generadas antes de la cancelación. Cuando corresponda legalmente, la cancelación podrá efectuarse
              mediante el mismo medio utilizado para la contratación. Las condiciones específicas de reembolso, cuando
              correspondan, serán informadas al momento de contratar y estarán sujetas a la legislación aplicable.
            </p>

            <h2>19. Disponibilidad de la Plataforma</h2>
            <p>TuOficioPro procurará mantener la Plataforma disponible y funcionando correctamente. Sin embargo, el Usuario reconoce que pueden producirse interrupciones derivadas de:</p>
            <ul>
              <li>mantenimiento;</li>
              <li>actualizaciones;</li>
              <li>fallas de servidores;</li>
              <li>problemas de conectividad;</li>
              <li>fallas de proveedores externos;</li>
              <li>ataques informáticos;</li>
              <li>circunstancias de fuerza mayor;</li>
              <li>hechos fuera del control razonable de TuOficioPro.</li>
            </ul>
            <p>
              TuOficioPro podrá modificar, suspender temporalmente o discontinuar determinadas funcionalidades, procurando
              informar a los Usuarios cuando resulte razonablemente posible.
            </p>

            <h2>20. Limitación de responsabilidad</h2>
            <p>
              TuOficioPro no será responsable por el incumplimiento, cumplimiento defectuoso, daños o perjuicios derivados
              directamente de los servicios contratados entre Clientes y Profesionales cuando TuOficioPro no sea parte de dicha
              contratación. En particular, TuOficioPro no garantiza la calidad del trabajo, la experiencia del Profesional,
              la disponibilidad del Profesional, el cumplimiento de los plazos, el precio acordado, la seguridad del lugar
              de prestación, la correcta ejecución del servicio ni el pago entre Cliente y Profesional.
            </p>
            <p>
              Sin perjuicio de lo anterior, ninguna disposición de estos Términos pretende excluir, limitar o restringir
              derechos o responsabilidades que no puedan ser legalmente excluidos o limitados conforme a la legislación
              aplicable. Las limitaciones establecidas en este artículo se aplicarán únicamente en la medida permitida por
              la legislación vigente.
            </p>

            <h2>21. Relaciones de consumo</h2>
            <p>
              Cuando una relación determinada quede comprendida dentro de la legislación argentina de defensa del
              consumidor, se aplicarán las normas imperativas correspondientes. Nada de estos Términos deberá interpretarse
              como una renuncia del Usuario a derechos que legalmente sean irrenunciables. En caso de contradicción entre
              una disposición de estos Términos y una norma legal de carácter imperativo aplicable a una relación
              determinada, prevalecerá la norma legal.
            </p>

            <h2>22. Propiedad intelectual</h2>
            <p>
              Todos los elementos propios de TuOficioPro, incluyendo su marca, nombre comercial, diseño, logotipo, software,
              código, estructura, textos, gráficos y demás contenidos desarrollados por TuOficioPro, están protegidos por las
              normas aplicables de propiedad intelectual. No está permitido copiar, reproducir, modificar, distribuir,
              vender o explotar comercialmente dichos elementos sin autorización previa, salvo cuando la legislación
              permita expresamente su utilización. El contenido publicado por los Usuarios continuará perteneciendo a sus
              respectivos titulares, sin perjuicio de las licencias necesarias para que TuOficioPro pueda prestar y promocionar
              sus servicios.
            </p>

            <h2>23. Datos personales y privacidad</h2>
            <p>
              TuOficioPro podrá recopilar y tratar datos personales de sus Usuarios de acuerdo con la legislación argentina
              aplicable en materia de protección de datos personales. El tratamiento de los datos se regirá por la
              <RouterLink to="/politica-de-privacidad">Política de Privacidad</RouterLink> independiente, que formará parte
              de la documentación legal de la Plataforma.
            </p>

            <h2>24. Comunicaciones comerciales</h2>
            <p>
              Cuando el Usuario preste su consentimiento o cuando la legislación aplicable lo permita, TuOficioPro podrá
              enviar comunicaciones relacionadas con novedades, promociones, funcionalidades, servicios, beneficios e
              información comercial. El Usuario podrá solicitar dejar de recibir comunicaciones comerciales escribiendo a
              <strong>{{ contactEmail }}</strong>. Las comunicaciones necesarias para la administración de la cuenta,
              seguridad, facturación o funcionamiento del servicio podrán continuar siendo enviadas cuando resulten
              necesarias.
            </p>

            <h2>25. Suspensión y cancelación de cuentas</h2>
            <p>TuOficioPro podrá suspender o cancelar una cuenta cuando existan motivos razonables para considerar que el Usuario:</p>
            <ul>
              <li>incumplió estos Términos;</li>
              <li>proporcionó información falsa;</li>
              <li>realizó actividades fraudulentas;</li>
              <li>puso en riesgo la seguridad de otros Usuarios;</li>
              <li>utilizó la Plataforma para actividades ilegales;</li>
              <li>manipuló reseñas o información;</li>
              <li>infringió derechos de terceros.</li>
            </ul>
            <p>
              Cuando resulte razonablemente posible, TuOficioPro podrá informar al Usuario los motivos de la medida. La
              suspensión o cancelación no afectará derechos u obligaciones que, por su naturaleza, deban continuar vigentes.
            </p>

            <h2>26. Eliminación de la cuenta y datos</h2>
            <p>
              El Usuario podrá solicitar la eliminación de su cuenta escribiendo a <strong>{{ contactEmail }}</strong>.
              La eliminación de una cuenta no implica necesariamente la eliminación inmediata de toda información
              relacionada con ella cuando exista una obligación legal, contractual, contable, fiscal, de seguridad o de
              defensa de derechos que justifique su conservación. El tratamiento posterior de los datos se realizará conforme
              a la Política de Privacidad y la legislación aplicable.
            </p>

            <h2>27. Enlaces y servicios de terceros</h2>
            <p>
              TuOficioPro podrá incorporar enlaces, servicios, herramientas o integraciones proporcionadas por terceros
              (por ejemplo, WhatsApp para contacto o soporte). TuOficioPro no controla necesariamente el funcionamiento,
              contenido, políticas o condiciones de dichos terceros. El uso de servicios externos podrá estar sujeto a sus
              propios términos y políticas.
            </p>

            <h2>28. Modificaciones de la Plataforma</h2>
            <p>
              TuOficioPro podrá modificar, actualizar o incorporar funcionalidades, categorías, planes, herramientas y
              servicios. Las modificaciones podrán realizarse para mejorar la seguridad, funcionamiento, experiencia de
              usuario o modelo comercial de la Plataforma. Cuando una modificación afecte sustancialmente una contratación
              vigente, TuOficioPro respetará los derechos que correspondan al Usuario conforme a la legislación aplicable.
            </p>

            <h2>29. Indemnidad</h2>
            <p>
              El Usuario será responsable por los daños que pudiera causar a TuOficioPro como consecuencia de un uso ilícito
              de la Plataforma o del incumplimiento de estos Términos, siempre dentro de los límites y condiciones
              establecidos por la legislación aplicable. Esta disposición no pretende trasladar al Usuario responsabilidades
              que legalmente correspondan a TuOficioPro ni limitar derechos reconocidos por normas imperativas.
            </p>

            <h2>30. Legislación aplicable</h2>
            <p>
              Estos Términos se regirán por las leyes de la República Argentina. Cuando corresponda la aplicación de normas
              de protección del consumidor, se respetarán las reglas imperativas de competencia, jurisdicción y protección
              establecidas por dichas normas. Ninguna disposición de estos Términos pretende privar al consumidor del
              acceso a los mecanismos administrativos o judiciales que legalmente le correspondan.
            </p>

            <h2>31. Resolución de conflictos</h2>
            <p>
              TuOficioPro procurará facilitar canales de contacto para que los Usuarios puedan informar problemas relacionados
              con la utilización de la Plataforma. En caso de conflictos entre Clientes y Profesionales, las partes deberán
              intentar resolver inicialmente la cuestión entre ellas, sin perjuicio de los derechos que les correspondan
              conforme a la legislación vigente. TuOficioPro podrá colaborar razonablemente cuando corresponda, pero su
              intervención no implica asumir responsabilidad por el contrato celebrado entre Cliente y Profesional.
            </p>

            <h2>32. Nulidad parcial</h2>
            <p>
              Si alguna disposición de estos Términos fuera declarada inválida, ilegal o inaplicable, dicha circunstancia no
              afectará la validez del resto de las disposiciones. La disposición afectada será interpretada o reemplazada,
              cuando corresponda, de manera compatible con la finalidad original y con la legislación aplicable.
            </p>

            <h2>33. Integridad contractual</h2>
            <p>
              Estos Términos, junto con la Política de Privacidad, las condiciones particulares de cada servicio y las
              demás políticas publicadas por TuOficioPro, constituyen el marco contractual aplicable al uso de la Plataforma.
              En caso de existir condiciones específicas para una determinada funcionalidad, dichas condiciones prevalecerán
              respecto de esa funcionalidad en caso de contradicción.
            </p>

            <h2>34. Contacto</h2>
            <p>Para consultas, reclamos o comunicaciones relacionadas con estos Términos:</p>
            <ul>
              <li>TuOficioPro</li>
              <li>Email: <strong>{{ contactEmail }}</strong></li>
              <li>Ubicación: San Miguel de Tucumán, Tucumán, Argentina</li>
            </ul>
          </div>

          <div class="legales-acciones">
            <button @click="irAHome" class="cta">Volver al Inicio</button>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { ROUTES, APP_CONFIG } from '../utils/constants'

const router = useRouter()
const fechaActualizacion = ref('27 de agosto de 2026')
const contactEmail = APP_CONFIG.CONTACT_EMAIL

const irAHome = () => {
  router.push(ROUTES.HOME)
}
</script>

<style scoped>
.terminos-condiciones {
  min-height: 100vh;
}

.legales-section {
  background: #f8fafc;
  min-height: calc(100vh - 80px);
  padding: 40px 24px 60px;
}

.legales-container {
  max-width: 800px;
  margin: 0 auto;
  padding-top: 0;
}

.legales-card {
  padding: 40px;
  cursor: default;
}

.legales-card:hover {
  transform: none;
}

.legales-titulo {
  text-align: center;
  margin-bottom: 30px;
}

.legales-fecha {
  text-align: center;
  margin-bottom: 30px;
  color: var(--muted);
}

.content {
  line-height: 1.6;
}

.legales-acciones {
  text-align: center;
  margin-top: 40px;
}

.content h2 {
  color: var(--text);
  margin-top: 30px;
  margin-bottom: 15px;
  font-size: 1.2em;
}

.content p {
  margin-bottom: 15px;
  color: var(--muted);
}

.content ul {
  margin-bottom: 20px;
  padding-left: 20px;
}

.content li {
  margin-bottom: 8px;
  color: var(--muted);
}

.content strong {
  color: var(--text);
}

.content a {
  color: var(--primary, #2563eb);
}

@media (max-width: 600px) {
  .legales-section {
    padding: 12px 0 48px;
  }

  .legales-container {
    max-width: 100%;
    padding-left: 10px;
    padding-right: 10px;
  }

  .legales-card {
    padding: 18px 14px;
    border-radius: 12px;
  }

  .legales-titulo {
    font-size: 1.35rem;
    margin-bottom: 16px;
  }

  .legales-fecha {
    margin-bottom: 20px;
    font-size: 0.875rem;
  }

  .content h2 {
    font-size: 1.05rem;
    margin-top: 22px;
  }
}
</style>
