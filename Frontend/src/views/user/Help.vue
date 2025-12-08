<template>
  <div class="max-w-4xl mx-auto px-4 py-10 text-[#101518] font-sans">
    <!-- Título principal -->
    <h1 class="text-4xl font-bold mb-2">Centro de ayuda</h1>
    <p class="text-sm text-gray-500 mb-8">Última actualización: 31 de julio de 2025</p>

    <!-- Sección: Cuenta -->
    <section class="mb-10">
      <h2 class="text-2xl font-semibold mb-4">Cuenta</h2>
      <div id="accordion-cuenta">
        <div class="border border-gray-200 rounded-lg divide-y divide-gray-200">
          <details class="group">
            <summary class="flex justify-between items-center cursor-pointer px-4 py-3 hover:bg-gray-50">
              <span class="font-medium">¿Cómo cambio mi contraseña?</span>
              <i class="fas fa-chevron-down transition-transform group-open:rotate-180"></i>
            </summary>
            <div class="px-4 pb-4 text-sm text-gray-600">
              Puedes cambiar tu contraseña desde la sección "Mi cuenta" → "Seguridad". Allí encontrarás un botón para actualizarla.
            </div>
          </details>

          <details class="group">
            <summary class="flex justify-between items-center cursor-pointer px-4 py-3 hover:bg-gray-50">
              <span class="font-medium">¿Cómo actualizo mi dirección de correo?</span>
              <i class="fas fa-chevron-down transition-transform group-open:rotate-180"></i>
            </summary>
            <div class="px-4 pb-4 text-sm text-gray-600">
              En "Mi cuenta" → "Datos personales", podés modificar tu correo electrónico. Recordá verificar el nuevo correo después.
            </div>
          </details>
        </div>
      </div>
    </section>

    <!-- Sección: Reservas -->
    <section class="mb-10">
      <h2 class="text-2xl font-semibold mb-4">Reservas</h2>
      <div id="accordion-reservas">
        <div class="border border-gray-200 rounded-lg divide-y divide-gray-200">
          <details class="group">
            <summary class="flex justify-between items-center cursor-pointer px-4 py-3 hover:bg-gray-50">
              <span class="font-medium">¿Cómo reservo una cancha?</span>
              <i class="fas fa-chevron-down transition-transform group-open:rotate-180"></i>
            </summary>
            <div class="px-4 pb-4 text-sm text-gray-600">
              Ingresá a la sección "Reservar", seleccioná el complejo, el día y el horario disponible. Luego confirmá tu reserva.
            </div>
          </details>

          <details class="group">
            <summary class="flex justify-between items-center cursor-pointer px-4 py-3 hover:bg-gray-50">
              <span class="font-medium">¿Puedo cancelar una reserva?</span>
              <i class="fas fa-chevron-down transition-transform group-open:rotate-180"></i>
            </summary>
            <div class="px-4 pb-4 text-sm text-gray-600">
              Sí, podés cancelar tu reserva desde la sección "Mis reservas" hasta 2 horas antes del horario agendado.
            </div>
          </details>
        </div>
      </div>
    </section>

    <!-- Sección: Soporte técnico -->
    <section class="mb-10">
      <h2 class="text-2xl font-semibold mb-4">Soporte técnico</h2>
      <div id="accordion-soporte">
        <div class="border border-gray-200 rounded-lg divide-y divide-gray-200">
          <details class="group">
            <summary class="flex justify-between items-center cursor-pointer px-4 py-3 hover:bg-gray-50">
              <span class="font-medium">No puedo iniciar sesión</span>
              <i class="fas fa-chevron-down transition-transform group-open:rotate-180"></i>
            </summary>
            <div class="px-4 pb-4 text-sm text-gray-600">
              Asegurate de estar usando el correo correcto. Si olvidaste tu contraseña, usá la opción "¿Olvidaste tu contraseña?" en la pantalla de inicio.
            </div>
          </details>

          <details class="group">
            <summary class="flex justify-between items-center cursor-pointer px-4 py-3 hover:bg-gray-50">
              <span class="font-medium">¿Cómo reporto un problema?</span>
              <i class="fas fa-chevron-down transition-transform group-open:rotate-180"></i>
            </summary>
            <div class="px-4 pb-4 text-sm text-gray-600">
              Podés escribirnos a soporte@tuapp.com o usar el botón de "Enviar reporte" en el menú lateral.
            </div>
          </details>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
export default {
  name: 'Help'
}
</script>
