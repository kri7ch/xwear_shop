<template>
  <div class="checkout-page">
    <h1 class="checkout-title">Оформление заказа</h1>
    <div class="checkout-steps">
      <div class="checkout-step done">Корзина</div>
      <div class="checkout-sep"></div>
      <div class="checkout-step active">Доставка</div>
      <div class="checkout-sep"></div>
      <div class="checkout-step active">Оплата</div>
      <div class="checkout-sep"></div>
      <div class="checkout-step">Подтверждение</div>
    </div>
    <div class="checkout-grid">
      <section class="checkout-summary">
        <h2>Ваш заказ</h2>
        <div v-if="loading" class="loading">Загрузка корзины...</div>
        <div v-else>
          <div v-if="cartItems.length === 0" class="empty">Корзина пуста</div>
          <ul v-else class="checkout-items">
            <li v-for="item in cartItems" :key="item.id" class="checkout-item">
              <img :src="item.imageUrl" alt="" class="checkout-thumb" />
              <div class="checkout-info">
                <div class="checkout-name">{{ item.brand }} {{ item.name }}</div>
                <div class="checkout-meta">Размер: {{ item.size }} • Кол-во: {{ item.quantity }}</div>
              </div>
              <div class="checkout-price">{{ (item.lineTotal ?? (item.price * item.quantity)).toFixed(2) }} ₽</div>
            </li>
          </ul>
          <div class="checkout-total" v-if="cartItems.length">
            <h3>Итого:</h3>
            <strong>{{ totalPrice.toFixed(2) }} ₽</strong>
          </div>
        </div>
      </section>

      <section class="checkout-form">
        <h2>Данные для доставки</h2>
        <div class="form-grid">
          <div class="field">
            <label>Страна</label>
            <input v-model.trim="country" type="text" placeholder="Россия" />
          </div>
          <div class="field">
            <label>Город</label>
            <input v-model.trim="city" type="text" placeholder="Москва" />
          </div>
          <div class="field">
            <label>Улица</label>
            <input v-model.trim="street" type="text" placeholder="Тверская" />
          </div>
          <div class="field">
            <label>Дом</label>
            <input v-model.trim="numberHome" type="text" placeholder="12" />
          </div>
          <div class="field">
            <label>Район</label>
            <input v-model.trim="district" type="text" placeholder="ЦАО" />
          </div>
          <div class="field">
            <label>Индекс</label>
            <input v-model.trim="index" type="text" placeholder="101000" />
          </div>
        </div>

        <h2>Способ оплаты</h2>
        <div class="payment">
          <label>
            <input type="radio" value="cash" v-model="paymentMethod" /> Наличные
          </label>
          <label>
            <input type="radio" value="card" v-model="paymentMethod" /> Карта
          </label>
        </div>

        <div v-if="paymentMethod === 'card'" class="checkout-card-fields">
          <div class="field">
            <label>Номер карты</label>
            <input v-model.trim="cardNumber" type="text" placeholder="4111 1111 1111 1111" />
          </div>
          <div class="field">
            <label>Имя на карте</label>
            <input v-model.trim="cardName" type="text" placeholder="IVANOV IVAN" />
          </div>
          <div class="checkout-row">
            <div class="field">
              <label>Месяц</label>
              <input v-model.trim="cardMonth" type="text" placeholder="MM" />
            </div>
            <div class="field">
              <label>Год</label>
              <input v-model.trim="cardYear" type="text" placeholder="YY" />
            </div>
            <div class="field">
              <label>CVV</label>
              <input v-model.trim="cardCvv" type="password" placeholder="123" />
            </div>
          </div>
        </div>

        <button class="checkout-place-order" :disabled="submitting || cartItems.length === 0" @click="placeOrder">
          {{ submitting ? 'Оформляем...' : 'Оформить заказ' }}
        </button>
      </section>
    </div>
  </div>
</template>

<script>
import { showToast } from '../utils/toast'
import { setCartCount } from '../utils/cartBadge'

export default {
  name: 'CheckoutPage',
  data() {
    return {
      cartItems: [],
      loading: true,
      submitting: false,

      country: '',
      street: '',
      city: '',
      numberHome: '',
      district: '',
      index: '',

      paymentMethod: 'cash',
      cardNumber: '',
      cardName: '',
      cardMonth: '',
      cardYear: '',
      cardCvv: ''
    }
  },
  computed: {
    totalPrice() {
      return this.cartItems.reduce((sum, i) => sum + (i.lineTotal ?? (i.price * i.quantity)), 0)
    }
  },
  methods: {
    async loadCart() {
      this.loading = true
      try {
        const res = await fetch('/api/cart', { credentials: 'include' })
        if (!res.ok) throw new Error('cart_failed')
        const data = await res.json()
        this.cartItems = data.items || []
      } catch (e) {
        showToast('error', 'Не удалось загрузить корзину')
      } finally {
        this.loading = false
      }
    },
    validateAddress() {
      const required = [this.country, this.street, this.city, this.numberHome, this.district, this.index]
      return required.every(v => v && v.trim().length > 0)
    },
    validateCard() {
      if (this.paymentMethod !== 'card') return true
      const num = this.cardNumber.replace(/\s+/g, '')
      const mm = parseInt(this.cardMonth, 10)
      const cvv = this.cardCvv
      if (!/^\d{16}$/.test(num)) return false
      if (!(mm >= 1 && mm <= 12)) return false
      if (!/^\d{2}$/.test(this.cardYear)) return false
      if (!/^\d{3}$/.test(cvv)) return false
      return true
    },
    async placeOrder() {
      if (!this.validateAddress()) {
        showToast('error', 'Заполните все поля адреса')
        return
      }
      if (!this.validateCard()) {
        showToast('error', 'Проверьте данные карты')
        return
      }
      this.submitting = true
      try {
        const payload = {
          country: this.country,
          street: this.street,
          city: this.city,
          numberHome: this.numberHome,
          district: this.district,
          index: this.index
        }
        const res = await fetch(`/api/orders?payment=${encodeURIComponent(this.paymentMethod)}`, {
          method: 'POST',
          credentials: 'include',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload)
        })
        if (!res.ok) {
          const err = await res.json().catch(() => ({}))
          if (err && err.message) {
            const map = {
              address_invalid: 'Заполните корректно адрес доставки',
              cart_empty: 'Ваша корзина пуста',
              payment_invalid: 'Неверный способ оплаты'
            }
            showToast('error', map[err.message] || 'Ошибка оформления заказа')
          } else {
            showToast('error', 'Ошибка оформления заказа')
          }
          return
        }
        const { id } = await res.json()
        setCartCount(0)
        showToast('success', `Заказ #${id} успешно оформлен`)
        this.$router.push({ name: 'Account' })
      } catch (e) {
        showToast('error', 'Ошибка сети при оформлении заказа')
      } finally {
        this.submitting = false
      }
    }
  },
  mounted() {
    this.loadCart()
  }
}
</script>
