<template>
  <div id="app">
    <AppNavbar />
    <main class="app-main">
      <router-view />
    </main>
    <AppFooter />
    <ToastContainer />
  </div>
</template>

<script>
import AppNavbar from './components/Navbar.vue';
import AppFooter from './components/Footer.vue';
import ToastContainer from './components/ToastContainer.vue'
import './style/style.css'

export default {
  name: 'App',
  components: {
    AppNavbar,
    AppFooter,
    ToastContainer
  }
}
</script>

<style>
html, body, #app { 
    min-height: 100vh;
}

#app { 
  display: flex; 
  flex-direction: column; 
}
.app-main { 
  flex: 1 0 auto; 
}
</style>
