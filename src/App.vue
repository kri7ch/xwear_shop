<template>
  <div id="app">
    <AppNavbar />
    <router-view />
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