<template>
  <div id="app">
    <AppNavbar />
    <router-view />
  </div>
</template>

<script>
import AppNavbar from './components/Navbar.vue';
import './style/style.css'

export default {
  name: 'App',
  components: {
    AppNavbar,
  }
}
</script>