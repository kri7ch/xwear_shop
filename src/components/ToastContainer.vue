<template>
  <div class="toast-container">
    <div v-for="t in toasts" :key="t.id" :class="['toast', t.type]">
      <span class="toast-message">{{ t.message }}</span>
      <button class="toast-close" @click="remove(t.id)">×</button>
    </div>
  </div>
</template>

<script>
import { subscribeToast } from '../utils/toast'

export default {
  name: 'ToastContainer',
  data() {
    return { toasts: [], unsubscribe: null }
  },
  mounted() {
    this.unsubscribe = subscribeToast((t) => {
      this.toasts.push(t)
      setTimeout(() => this.remove(t.id), t.timeout || 3000)
    })
  },
  beforeUnmount() {
    if (this.unsubscribe) this.unsubscribe()
  },
  methods: {
    remove(id) {
      this.toasts = this.toasts.filter(x => x.id !== id)
    }
  }
}
</script>