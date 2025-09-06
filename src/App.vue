<template>
  <div id="app">
    <AppNavbar />
    <router-view />
    <AppFooter />
  </div>
</template>

<script>
import AppNavbar from './components/Navbar.vue';
import AppFooter from './components/Footer.vue';
import './style/style.css'

export default {
  name: 'App',
  components: {
    AppNavbar,
    AppFooter,
  }
}
</script>