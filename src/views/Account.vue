<template>
  <section class="account-page">
    <template v-if="!isAuth && mode === 'login'">
      <h1 class="account-title">АККАУНТ</h1>
      <div class="account-panels">
        <div class="account-panel">
          <h2 class="panel-title">Войти</h2>
          <form class="account-form" @submit.prevent="onLogin">
            <div class="form-group">
              <label class="form-label">Email адрес:</label>
              <input class="form-input" type="email" placeholder="email@example.com" v-model="loginEmail" />
            </div>
            <div class="form-group">
              <label class="form-label">Пароль:</label>
              <input class="form-input" type="password" placeholder="*********************" v-model="loginPassword" />
            </div>
            <div class="form-row">
              <label class="checkbox">
                <input type="checkbox" />
                <span>Запомнить меня</span>
              </label>
              <a class="forgot-link" href="#">Забыли пароль?</a>
            </div>
            <button class="submit-btn-enter" type="submit">ВОЙТИ</button>
          </form>
        </div>
        <div class="account-panel">
          <h2 class="panel-title">Регистрация</h2>
          <form class="account-form" @submit.prevent="onRegister">
            <div class="form-group">
              <label class="form-label">Email адрес:</label>
              <input class="form-input" type="email" placeholder="email@example.com" v-model="regEmail"/>
            </div>
            <div class="form-group">
              <label class="form-label">Пароль:</label>
              <input class="form-input" type="password" placeholder="*********************" v-model="regPassword"/>
            </div>
            <div class="form-group">
              <label class="form-label">Повторите пароль:</label>
              <input class="form-input" type="password" placeholder="*********************" v-model="regPasswordRepeat"/>
            </div>
            <button class="submit-btn-reg" type="submit">ЗАРЕГИСТРИРОВАТЬСЯ</button>
          </form>
        </div>
      </div>
    </template>

    <template v-else>
      <div class="profile-layout">
        <aside class="profile-sidebar">
          <ul>
            <li class="with-icon" :class="{active: activeTab==='profile'}" @click="activeTab='profile'">
              <img class="icon-img" src="../assets/images/icons/profile.svg" alt="Профиль" />
              <span>Мой профиль</span>
            </li>
            <li class="with-icon" :class="{active: activeTab==='editProfile'}" @click="activeTab='editProfile'">
              <img class="icon-img" src="../assets/images/icons/edit_profile.svg" alt="Редактировать профиль" />
              <span>Редактировать профиль</span>
            </li>
            <li class="with-icon" :class="{active: activeTab==='ordersHistory'}" @click="activeTab='ordersHistory'">
              <img class="icon-img" src="../assets/images/icons/order_history.svg" alt="История заказов" />
              <span>История заказов</span>
            </li>
            <li class="with-icon" :class="{active: activeTab==='orders'}" @click="activeTab='orders'">
              <img class="icon-img" src="../assets/images/icons/orders.svg" alt="Мои заказы" />
              <span>Мои заказы</span>
            </li>
            <li class="with-icon" :class="{active: activeTab==='addresses'}" @click="activeTab='addresses'">
              <img class="icon-img" src="../assets/images/icons/addres.svg" alt="Адреса" />
              <span>Адреса</span>
            </li>
            <li class="with-icon" :class="{active: activeTab==='editAddresses'}" @click="activeTab='editAddresses'">
              <img class="icon-img" src="../assets/images/icons/edit_addres.svg" alt="Редактировать адреса" />
              <span>Редактировать адреса</span>
            </li>
            <li class="with-icon" :class="{active: activeTab==='password'}" @click="activeTab='password'">
              <img class="icon-img" src="../assets/images/icons/password.svg" alt="Пароль" />
              <span>Пароль</span>
            </li>
            <li class="with-icon logout" @click="openLogoutConfirm">
              <img class="icon-img" src="../assets/images/icons/exit.svg" alt="Выход" />
              <span>Выход</span>
            </li>
          </ul>
        </aside>
        
        <main class="profile-main">
          <h1 class="profile-title">ЛИЧНЫЙ КАБИНЕТ</h1>
          <p class="welcome">Приветствуем, {{ user?.name || user?.email }}!</p>
          <div class="tiles">
            <div class="tile" :class="{ active: activeTab==='profile' }" @click="activeTab='profile'">
              <img class="tile-icon-img" src="../assets/images/icons/profile.svg" alt="Мой профиль" />
              <div class="tile-text">Мой профиль</div>
            </div>
            <div class="tile" :class="{ active: activeTab==='orders' }" @click="activeTab='orders'">
              <img class="tile-icon-img" src="../assets/images/icons/orders.svg" alt="Заказы" />
              <div class="tile-text">Заказы</div>
            </div>
            <div class="tile" :class="{ active: activeTab==='addresses' }" @click="activeTab='addresses'">
              <img class="tile-icon-img" src="../assets/images/icons/addres.svg" alt="Мои адреса" />
              <div class="tile-text">Мои адреса</div>
            </div>
            <div class="tile" :class="{ active: activeTab==='editProfile' }" @click="activeTab='editProfile'">
              <img class="tile-icon-img" src="../assets/images/icons/edit_profile.svg" alt="Редактировать профиль" />
              <div class="tile-text">Редактировать профиль</div>
            </div>
            <div class="tile" :class="{ active: activeTab==='favourites' }" @click="activeTab='favourites'">
              <img class="tile-icon-img" src="../assets/images/icons/favourites.svg" alt="Избранные товары" />
              <div class="tile-text">Избранные товары</div>
            </div>
            <div class="tile logout" @click="openLogoutConfirm">
              <img class="tile-icon-img" src="../assets/images/icons/exit.svg" alt="Выход" />
              <div class="tile-text">Выход</div>
            </div>
          </div>

          <section class="profile-info" v-if="activeTab === 'profile'">
            <h2 class="profile-info-title">Мой профиль</h2>
            <div class="profile-card">
              <ul class="info-list">
                <li class="info-item"><span class="info-label">Имя</span><span class="info-value">{{ user?.name || '—' }}</span></li>
                <li class="info-item"><span class="info-label">Email</span><span class="info-value">{{ user?.email }}</span></li>
              </ul>
            </div>
          </section>

          <section class="edit-profile" v-if="activeTab === 'editProfile'">
            <h2 class="profile-info-title">Редактировать профиль</h2>
            <form class="account-form-edit" @submit.prevent="onUpdateProfile">
              <div class="form-group">
                <label class="form-label">Имя:</label>
                <input class="form-input" type="text" v-model="editName" placeholder="Ваше имя" />
              </div>
              <div class="form-group">
                <label class="form-label">Email адрес:</label>
                <input class="form-input" type="email" v-model="editEmail" placeholder="email@example.com" />
              </div>
              <div class="form-group">
                <label class="form-label">Телефон:</label>
                <input class="form-input" type="tel" v-model="editPhone" placeholder="+7…" />
              </div>
              <button class="submit-btn-enter" type="submit">Сохранить</button>
            </form>
          </section>

          <section class="change-password" v-if="activeTab === 'password'">
            <h2 class="profile-info-title">Сменить пароль</h2>
            <form class="account-form-edit" @submit.prevent="onChangePassword">
              <div class="form-group">
                <label class="form-label">Текущий пароль:</label>
                <input class="form-input" type="password" v-model="passwordCurrent" placeholder="*********************" />
              </div>
              <div class="form-group">
                <label class="form-label">Новый пароль:</label>
                <input class="form-input" type="password" v-model="passwordNew" placeholder="минимум 6 символов" />
              </div>
              <div class="form-group">
                <label class="form-label">Повторите новый пароль:</label>
                <input class="form-input" type="password" v-model="passwordRepeat" placeholder="*********************" />
              </div>
              <button class="submit-btn-enter" type="submit">Обновить пароль</button>
            </form>
          </section>

          <section class="orders" v-if="activeTab === 'orders'">
            <h2>Текущие заказы</h2>
            <div v-if="orders.length === 0" class="orders-empty">У вас пока нет заказов</div>
            <table v-else class="orders-table">
              <thead>
                <tr>
                  <th>Номер</th>
                  <th>Дата</th>
                  <th>Статус</th>
                  <th>Итог</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="order in orders" :key="order.id">
                  <td>#{{ order.id }}</td>
                  <td>{{ order.date }}</td>
                  <td>{{ order.status }}</td>
                  <td>{{ order.totalAmount }} ₽</td>
                </tr>
              </tbody>
            </table>
          </section>

          <section class="orders-history" v-if="activeTab === 'ordersHistory'">
            <h2 style="margin: 0 0 12px;">История заказов</h2>
            <div v-if="ordersHistory.length === 0" class="orders-empty">У вас пока нет заказов</div>
            <table v-else class="orders-table">
              <thead>
                <tr>
                  <th>Номер</th>
                  <th>Дата</th>
                  <th>Статус</th>
                  <th>Итог</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="order in ordersHistory" :key="order.id">
                  <td>#{{ order.id }}</td>
                  <td>{{ order.date }}</td>
                  <td>{{ order.status }}</td>
                  <td>{{ order.totalAmount }} ₽</td>
                </tr>
              </tbody>
            </table>
          </section>

          <section class="addresses" v-if="activeTab === 'addresses'">
            <h2>Адреса</h2>
            <div class="address-card empty">
              <img class="address-icon" src="../assets/images/icons/addres.svg" alt="Адрес" />
              <div class="address-text">
                <div class="address-title">Добавьте адрес доставки</div>
                <div class="address-subtitle">Укажите город, улицу, дом, район и индекс</div>
              </div>
            </div>
          </section>

          <section class="edit-addresses" v-if="activeTab === 'editAddresses'">
            <h2>Редактировать адрес</h2>
            <div class="address-edit-card">
              <div class="address-edit-row">
                <input class="address-input" type="text" v-model="editAddress" placeholder="Адрес доставки" disabled />
                <button class="btn-primary" disabled>Сохранить</button>
              </div>
              <p class="disabled-note">Редактирование временно недоступно</p>
            </div>
          </section>
        </main>
      </div>
    </template>
    
    <div class="modal-overlay" v-if="showLogoutConfirm" @click.self="cancelLogout">
      <div class="modal">
        <h3 class="modal-title">Выйти из аккаунта?</h3>
        <div class="modal-actions">
          <button class="btn btn-secondary" @click="cancelLogout">Отмена</button>
          <button class="btn btn-danger" @click="confirmLogout">Выйти</button>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import { showToast } from '../utils/toast'

export default {
  name: 'AccountPage',
  data() {
    return {
      isAuth: false,
      user: null,
      mode: this.$route.query.mode || 'profile',
      activeTab: 'profile',

      showLogoutConfirm: false,

      loginEmail: '',
      loginPassword: '',

      regEmail: '',
      regPassword: '',
      regPasswordRepeat: '',

      editEmail: '',
      editName: '',
      editPhone: '',

      passwordCurrent: '',
      passwordNew: '',
      passwordRepeat: '',

      orders: [],
      ordersHistory: [],
      editAddress: ''
    }
  },
  async mounted() {
    await this.fetchMe()
  },
  methods: {
    openLogoutConfirm() {
      this.showLogoutConfirm = true
    },
    async loadOrders() {
      try {
        const res = await fetch('/api/orders', { credentials: 'include' })
        if (res.ok) {
          this.orders = await res.json()
        }
      } catch (e) {
        console.error('Не удалось загрузить заказы', e)
      }
    },
    async loadOrdersHistory() {
      try {
        const res = await fetch('/api/orders/history', { credentials: 'include' })
        if (res.ok) {
          this.ordersHistory = await res.json()
        }
      } catch (e) {
        console.error('Не удалось загрузить историю заказов', e)
      }
    },
    cancelLogout() {
      this.showLogoutConfirm = false
    },
    async confirmLogout() {
      await this.onLogout()
      this.showLogoutConfirm = false
    },
    async fetchMe() {
      try {
        const res = await fetch('/api/users/profile', { credentials: 'include' })
        if (res.ok) {
          this.user = await res.json()
          this.isAuth = true
          this.mode = 'profile'
          this.activeTab = 'profile'
          this.editEmail = this.user?.email || ''
          this.editName = this.user?.name || ''
          this.editPhone = this.user?.phone || ''
        } else {
          this.isAuth = false
        }
      } catch (e) {
        this.isAuth = false
      }
    },
    async onUpdateProfile() {
      const payload = {}
      const emailTrim = (this.editEmail || '').trim()
      if (emailTrim.length > 0) {
        payload.email = emailTrim
      }
      payload.name = this.editName
      payload.phone = this.editPhone
      try {
        const res = await fetch('/api/users/profile', {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          credentials: 'include',
          body: JSON.stringify(payload)
        })
        const data = await res.json().catch(() => null)
        if (!res.ok) {
          showToast('error', (data && data.message) || 'Ошибка обновления профиля')
          return
        }
        this.user = data
        showToast('success', 'Профиль обновлён')
        this.activeTab = 'profile'
      } catch (e) {
        showToast('error', 'Сеть недоступна или сервер недоступен')
      }
    },
    async onChangePassword() {
      if (this.passwordNew !== this.passwordRepeat) {
        showToast('error', 'Новые пароли не совпадают')
        return
      }
      try {
        const res = await fetch('/api/users/profile/password', {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          credentials: 'include',
          body: JSON.stringify({ currentPassword: this.passwordCurrent, newPassword: this.passwordNew })
        })
        const data = await res.json().catch(() => null)
        if (!res.ok) {
          showToast('error', (data && data.message) || 'Не удалось обновить пароль')
          return
        }
        showToast('success', 'Пароль обновлён')
        this.passwordCurrent = ''
        this.passwordNew = ''
        this.passwordRepeat = ''
        this.activeTab = 'profile'
      } catch (e) {
        showToast('error', 'Сеть недоступна или сервер недоступен')
      }
    },
    async onLogin() {
      try {
        const res = await fetch('/api/users/login', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          credentials: 'include',
          body: JSON.stringify({ email: this.loginEmail, password: this.loginPassword })
        })
        const data = await res.json().catch(() => null)
        if (!res.ok) {
          showToast('error', (data && data.message) || 'Ошибка входа')
          return
        }
        showToast('success', 'Успешный вход')
        await this.fetchMe()
        this.activeTab = 'profile'
        this.loginEmail = ''
        this.loginPassword = ''
        this.$router.replace({ name: 'Account' }).catch(() => {})
      } catch (e) {
        showToast('error', 'Сеть недоступна или сервер недоступен')
      }
    },
    async onRegister() {
      if (this.regPassword !== this.regPasswordRepeat) {
        showToast('error', 'Пароли не совпадают')
        return
      }
      try {
        const res = await fetch('/api/users/register', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ email: this.regEmail, password: this.regPassword })
        })
        const data = await res.json()
        if (!res.ok) {
          showToast('error', data?.message || 'Ошибка регистрации')
          return
        }
        showToast('success', 'Регистрация прошла успешно')
        this.regEmail = ''
        this.regPassword = ''
        this.regPasswordRepeat = ''
      } catch (e) {
        showToast('error', 'Сеть недоступна или сервер недоступен')
      }
    },
    async onLogout() {
      try {
        await fetch('/api/users/logout', { method: 'POST', credentials: 'include' })
      } catch (e) {
        console.error('Ошибка:', e)
      }
      this.isAuth = false
      this.user = null
      this.mode = 'login'
      this.activeTab = 'profile'
      this.$router.replace({ name: 'Account', query: { mode: 'login' } })
    }
  }
  ,
  watch: {
    activeTab(val) {
      if (val === 'orders') {
        this.loadOrders()
      } else if (val === 'ordersHistory') {
        this.loadOrdersHistory()
      }
    }
  }
}
</script>