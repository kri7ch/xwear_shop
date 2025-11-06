<template>
  <section class="cart-page">
    <div class="container">
      <h1 class="title">КОРЗИНА ТОВАРОВ</h1>

      <div v-if="!loading && authRequired" class="empty-state">
        <img class="empty-icon" src="../assets/images/icons/no-product-cart.svg" alt="Требуется вход" />
        <h2 class="empty-title">Авторизуйтесь, чтобы увидеть вашу корзину.</h2>
        <p class="empty-text">Корзина привязана к вашему аккаунту.</p>
        <button class="btn-primary" @click="goToLogin">ВОЙТИ</button>
      </div>

      <div v-else-if="!loading && (!cart.items || cart.items.length === 0)" class="empty-state">
        <img class="empty-icon" src="../assets/images/icons/no-product-cart.svg" alt="Пустая корзина" />
        <h2 class="empty-title">Ваша корзина на данный момент пуста.</h2>
        <p class="empty-text">Прежде чем приступить к оформлению заказа, добавьте некоторые товары в корзину.</p>
        <button class="btn-primary" @click="goToCatalog">ПЕРЕЙТИ В КАТАЛОГ</button>
      </div>

      <div v-else class="cart-content">
        <div class="items">
          <div class="item-card" v-for="(it, idx) in cart.items" :key="idx">
            <img class="item-image" :src="it.imageUrl" :alt="it.name" />
            <div class="item-info">
              <div class="item-name">{{ it.name }}</div>
              <div class="item-brand">Бренд: {{ it.brand }}</div>
              <div class="item-size">Размер: {{ it.size }}</div>
              <div class="item-price">Цена: {{ formatPrice(it.price) }}</div>
            </div>
            <div class="item-actions">
              <div class="qty-control">
                <button class="qty-btn" @click="decQty(it)">−</button>
                <input class="qty-input" type="number" min="1" :value="it.quantity" @input="onQtyInput(it, $event)" />
                <button class="qty-btn" @click="incQty(it)">+</button>
              </div>
              <div class="line-total">{{ formatPrice(it.lineTotal) }}</div>
              <button class="remove-btn" @click="removeItem(it)">Удалить</button>
            </div>
          </div>
        </div>

        <aside class="summary">
          <div class="summary-row">
            <span>Всего товаров</span>
            <span>{{ cart.totalItems }}</span>
          </div>
          <div class="summary-row total">
            <span>Итого</span>
            <span class="amount">{{ formatPrice(cart.totalPrice) }}</span>
          </div>
          <button class="btn-primary checkout" :disabled="cart.totalItems === 0" @click="goToCheckout">ОФОРМИТЬ ЗАКАЗ</button>
        </aside>
      </div>
    </div>
  </section>
</template>

<script>
export default {
  name: 'CartPage',
  data() {
    return {
      loading: false,
      authRequired: false,
      cart: { items: [], totalItems: 0, totalPrice: 0 }
    }
  },
  async mounted() {
    await this.loadCart()
  },
  methods: {
    async loadCart() {
      this.loading = true
      try {
        const res = await fetch('/api/cart', { credentials: 'include' })
        if (res.status === 401) {
          this.authRequired = true
          this.cart = { items: [], totalItems: 0, totalPrice: 0 }
        } else if (res.ok) {
          this.cart = await res.json()
        }
      } catch (e) {
        console.error('Не удалось загрузить корзину', e)
      } finally {
        this.loading = false
      }
    },
    formatPrice(n) {
      const v = Math.round(Number(n) || 0)
      try { return new Intl.NumberFormat('ru-RU').format(v) + ' ₽' } catch (_) { return v + ' ₽' }
    },
    async updateQuantity(productId, size, quantity) {
      try {
        const res = await fetch(`/api/cart/update?size=${encodeURIComponent(Number(size))}&quantity=${Number(quantity)}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          credentials: 'include',
          body: JSON.stringify({ productId: productId })
        })
        if (res.ok) {
          this.cart = await res.json()
        }
      } catch (e) {
        console.error('Не удалось обновить количество', e)
      }
    },
    incQty(it) {
      const q = Number(it.quantity) + 1
      this.updateQuantity(it.productId, it.size, q)
    },
    decQty(it) {
      const q = Number(it.quantity) - 1
      this.updateQuantity(it.productId, it.size, Math.max(0, q))
    },
    onQtyInput(it, ev) {
      const val = parseInt(ev.target.value, 10)
      if (isNaN(val)) return
      this.updateQuantity(it.productId, it.size, Math.max(0, val))
    },
    async removeItem(it) {
      try {
        const res = await fetch(`/api/cart/remove?size=${encodeURIComponent(Number(it.size))}`, {
          method: 'DELETE',
          headers: { 'Content-Type': 'application/json' },
          credentials: 'include',
          body: JSON.stringify({ productId: it.productId })
        })
        if (res.ok) {
          this.cart = await res.json()
        }
      } catch (e) {
        console.error('Не удалось удалить товар', e)
      }
    },
    goToCatalog() {
      this.$router.push('/catalog')
    },
    goToLogin() {
      this.$router.push({ name: 'Account', query: { mode: 'login' } })
    },
    goToCheckout() {
      if (this.cart.totalItems > 0) {
        this.$router.push({ name: 'Checkout' })
      }
    }
  }
}
</script>