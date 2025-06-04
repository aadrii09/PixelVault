<template>
  <div class="cookies-policy">
    <div class="policy-header">
      <h1>Política de Cookies</h1>
    </div>
    
    <div class="policy-content">
      <section>
        <h2>¿Qué son las cookies?</h2>
        <p>Las cookies son pequeños archivos de texto que los sitios web colocan en su dispositivo al visitar un sitio. Se utilizan ampliamente para hacer que los sitios web funcionen de manera más eficiente, así como para proporcionar información a los propietarios del sitio.</p>
      </section>
      
      <section>
        <h2>¿Cómo utilizamos las cookies?</h2>
        <p>En PixelVault utilizamos cookies por varias razones, incluyendo:</p>
        <ul>
          <li>Cookies esenciales: Necesarias para el funcionamiento básico del sitio.</li>
          <li>Cookies de preferencias: Para recordar información que cambia la forma en que se comporta o se ve el sitio.</li>
          <li>Cookies estadísticas: Nos ayudan a entender cómo los visitantes interactúan con el sitio.</li>
          <li>Cookies de marketing: Se utilizan para rastrear a los visitantes en los sitios web con el objetivo de mostrar anuncios relevantes.</li>
        </ul>
      </section>
      
      <section>
        <h2>Tipos de cookies que utilizamos</h2>
        <div class="cookie-tables">
          <div class="cookie-table">
            <h3>Cookies Esenciales</h3>
            <table>
              <thead>
                <tr>
                  <th>Cookie</th>
                  <th>Duración</th>
                  <th>Descripción</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>session_id</td>
                  <td>Sesión</td>
                  <td>Mantiene el estado de la sesión del usuario entre páginas</td>
                </tr>
                <tr>
                  <td>auth_token</td>
                  <td>30 días</td>
                  <td>Gestiona el inicio de sesión del usuario</td>
                </tr>
              </tbody>
            </table>
          </div>
          
          <div class="cookie-table">
            <h3>Cookies de Preferencias</h3>
            <table>
              <thead>
                <tr>
                  <th>Cookie</th>
                  <th>Duración</th>
                  <th>Descripción</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>theme</td>
                  <td>1 año</td>
                  <td>Guarda la preferencia de tema del usuario</td>
                </tr>
                <tr>
                  <td>language</td>
                  <td>1 año</td>
                  <td>Almacena la preferencia de idioma seleccionada</td>
                </tr>
              </tbody>
            </table>
          </div>
          
          <div class="cookie-table">
            <h3>Cookies Estadísticas</h3>
            <table>
              <thead>
                <tr>
                  <th>Cookie</th>
                  <th>Duración</th>
                  <th>Descripción</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>_ga</td>
                  <td>2 años</td>
                  <td>Registra un ID único para generar datos estadísticos sobre cómo el visitante usa el sitio web</td>
                </tr>
                <tr>
                  <td>_gid</td>
                  <td>24 horas</td>
                  <td>Registra un ID único para generar datos estadísticos sobre cómo el visitante usa el sitio web</td>
                </tr>
              </tbody>
            </table>
          </div>
          
          <div class="cookie-table">
            <h3>Cookies de Marketing</h3>
            <table>
              <thead>
                <tr>
                  <th>Cookie</th>
                  <th>Duración</th>
                  <th>Descripción</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>_fbp</td>
                  <td>3 meses</td>
                  <td>Utilizada por Facebook para entregar una serie de productos publicitarios</td>
                </tr>
                <tr>
                  <td>ads_prefs</td>
                  <td>1 año</td>
                  <td>Utilizada para recordar sus preferencias de publicidad</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </section>
      
      <section>
        <h2>Control de cookies</h2>
        <p>Puede configurar su navegador para que no acepte cookies, y el siguiente sitio web le indica cómo eliminar las cookies de su navegador. Sin embargo, es posible que algunas de nuestras funciones del sitio web no funcionen como resultado.</p>
        <p>La mayoría de los navegadores le permiten:</p>
        <ul>
          <li>Ver sus cookies</li>
          <li>Borrar las cookies</li>
          <li>Bloquear cookies de terceros</li>
          <li>Bloquear cookies de sitios particulares</li>
          <li>Bloquear todas las cookies</li>
          <li>Borrar todas las cookies cuando cierre su navegador</li>
        </ul>
        <div class="browser-links">
          <p>Puede encontrar información sobre cómo manejar las cookies en los siguientes navegadores:</p>
          <ul>
            <li><a href="https://support.google.com/chrome/answer/95647" target="_blank">Google Chrome</a></li>
            <li><a href="https://support.mozilla.org/es/kb/habilitar-y-deshabilitar-cookies-sitios-web-rastrear-preferencias" target="_blank">Firefox</a></li>
            <li><a href="https://support.microsoft.com/es-es/help/17442/windows-internet-explorer-delete-manage-cookies" target="_blank">Internet Explorer</a></li>
            <li><a href="https://support.apple.com/es-es/guide/safari/sfri11471/mac" target="_blank">Safari</a></li>
            <li><a href="https://help.opera.com/en/latest/web-preferences/#cookies" target="_blank">Opera</a></li>
          </ul>
        </div>
      </section>
      
      <section>
        <h2>Cambios a nuestra política de cookies</h2>
        <p>Cualquier cambio que podamos hacer a nuestra política de cookies en el futuro se publicará en esta página. Consulte esta política regularmente para mantenerse informado de las actualizaciones o cambios.</p>
      </section>
      
      <section>
        <h2>Contacto</h2>
        <p>Si tiene alguna pregunta sobre nuestra política de cookies, por favor contáctenos en:</p>
        <p>Email: pixelvaultassistant@gmail.com</p>
        <p>Teléfono: +34 900 123 123</p>
      </section>
      
      <div class="back-button">
        <button @click="$router.back()">Volver</button>
      </div>
    </div>
  </div>
</template>

<script setup>

</script>

<style scoped>

body {
  background: linear-gradient(135deg, #16213e 0%, #1a1a2e 100%);
}

.cookies-policy {
  text-align: justify;
  margin: 0 auto;
  padding: 12rem;
  color: white;
  background: linear-gradient(135deg, #16213e 0%, #1a1a2e 100%);
  min-height: 100vh;
}

.policy-header {
  text-align: center;
  margin-bottom: 3rem;
  border-bottom: 1px solid rgba(0, 255, 136, 0.3);
  padding-bottom: 1rem;
}

.policy-header h1 {
  font-size: 2.5rem;
  background: linear-gradient(45deg, #00ff88, #00ccff);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 1rem;
}

.policy-content {
  line-height: 1.8;
}

section {
  margin-bottom: 2.5rem;
}

h2 {
  font-size: 1.8rem;
  color: #00ccff;
  margin-bottom: 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  padding-bottom: 0.5rem;
}

h3 {
  font-size: 1.3rem;
  color: #00ff88;
  margin-bottom: 1rem;
}

p, li {
  color: rgba(255, 255, 255, 0.9);
  margin-bottom: 1rem;
}

ul {
  list-style-type: disc;
  margin-left: 2rem;
  margin-bottom: 1.5rem;
}

.cookie-tables {
  display: grid;
  grid-template-columns: 1fr;
  gap: 2rem;
}

@media (min-width: 768px) {
  .cookie-tables {
    grid-template-columns: repeat(2, 1fr);
  }
}

.cookie-table {
  margin-bottom: 2rem;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 1.5rem;
  background: rgba(0, 0, 0, 0.2);
  border-radius: 0.5rem;
  overflow: hidden;
}

thead {
  background: rgba(0, 204, 255, 0.2);
}

th, td {
  padding: 0.75rem;
  text-align: left;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

th {
  font-weight: bold;
  color: white;
}

.browser-links a {
  color: #00ff88;
  text-decoration: underline;
  transition: color 0.3s;
}

.browser-links a:hover {
  color: #00ccff;
}

.back-button {
  margin-top: 3rem;
  text-align: center;
}

.back-button button {
  background: linear-gradient(45deg, #00ff88, #00ccff);
  border: none;
  border-radius: 2rem;
  color: #16213e;
  font-weight: bold;
  padding: 0.75rem 2rem;
  cursor: pointer;
  transition: all 0.3s;
}

.back-button button:hover {
  box-shadow: 0 0 15px rgba(0, 255, 136, 0.5);
  transform: translateY(-2px);
}
</style>
