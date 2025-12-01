<template>
    <nav>
        <div class="nameShop">
            <router-link to="/mainpage" class="brand-link">
                <h1 class="shopName">XWEAR</h1>
            </router-link>
        </div>

        <div class="menu">
            <ul>
                <li><a href="#">Одежда</a>
                    <ul>
                        <li><a href="#">Женская одежда</a></li>
                        <li><a href="#">Мужская одежда</a></li>
                        <li><a href="#">Детская одежда</a></li>
                    </ul>
                </li>

                <li><a href="#">Обувь</a>
                    <ul>
                        <li><a href="#">Женская обувь</a></li>
                        <li><a href="#">Мужская обувь</a></li>
                        <li><a href="#">Спортивная обувь</a></li>
                    </ul>
                </li>

                <li><a href="#">Аксессуары</a>
                    <ul>
                        <li><a href="#">Сумки и рюкзаки</a></li>
                        <li><a href="#">Головные уборы</a></li>
                        <li><a href="#">Бижутерия</a></li>
                    </ul>
                </li>

                <li>
                    <a class="menu-caret" href="#">Бренды</a>
                    <ul>
                        <li><a href="#">Nike</a></li>
                        <li><a href="#">Adidas</a></li>
                        <li><a href="#">Zara</a></li>
                    </ul>
                </li>

                <li><a href="#">Расчет стоимости</a>
                    <ul>
                        <li><a href="#">Калькулятор доставки</a></li>
                        <li><a href="#">Таблица размеров</a></li>
                        <li><a href="#">Стоимость услуг</a></li>
                    </ul>
                </li>

                <li><a href="#">Информация</a>
                    <ul>
                        <li><a href="#">О нас</a></li>
                        <li><a href="#">Доставка и оплата</a></li>
                        <li><a href="#">Контакты</a></li>
                    </ul>
                </li>
            </ul>
        </div>

        <div class="link-icon">
            <a href="#">
                <img src="../assets/images/icons/search_Icon.svg" alt="">
            </a>

            <a href="#">
                <img src="../assets/images/icons/star_icon.svg" alt="">
            </a>

            <router-link to="/account" class="user-profile-link">
                <img src="../assets/images/icons/user_icon.svg" alt="">
            </router-link>

            <router-link to="/cart" class="bascket-info-link cart-link">
                <img src="../assets/images/icons/bascket_icon.svg" alt="">
                <span v-if="cartCount > 0" class="cart-dot" aria-hidden="true"></span>
            </router-link>
        </div>
    </nav>
</template>

<script>
import { subscribeCartCount } from '../utils/cartBadge'

export default {
  name: 'AppNavbar',
  data() {
    return { cartCount: 0 }
  },
  mounted() {
    this.loadCartCount()
    this._unsubCart = subscribeCartCount((count) => {
      this.cartCount = Math.max(0, Number(count) || 0)
    })
  },
  beforeUnmount() {
    if (this._unsubCart) this._unsubCart()
  },
  methods: {
    async loadCartCount() {
      try {
        const res = await fetch('/api/cart', { credentials: 'include' })
        if (res.ok) {
          const cart = await res.json().catch(() => null)
          const count = (cart && typeof cart.totalItems === 'number')
            ? cart.totalItems
            : (Array.isArray(cart?.items) ? cart.items.length : 0)
          this.cartCount = Math.max(0, Number(count) || 0)
        } else if (res.status === 401) {
          this.cartCount = 0
        }
      } catch (_) {
        this.cartCount = 0
      }
    }
  }
}
</script>
