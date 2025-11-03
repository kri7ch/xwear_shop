<template>
  <section class="product-page" v-if="loaded && product">
    <button class="back-btn" @click="goBack" aria-label="Назад">
      <span class="back-icon">←</span>
      <span class="back-text">Назад</span>
    </button>
    <div class="product-grid">
      <div class="gallery">
        <img class="main-image" :src="selectedImage" :alt="product.name" />
        <div class="thumbs" v-if="product.images && product.images.length">
          <img
            v-for="(img, idx) in product.images"
            :key="idx"
            class="thumb"
            :src="img"
            :alt="product.name + ' ' + (idx+1)"
            :class="{ active: img === selectedImage }"
            @click="selectedImage = img"
          />
        </div>
      </div>

      <div class="info">
        <h1 class="product-title">{{ product.name }}</h1>

        <div class="size-section">
          <h3 class="size-title">EU РАЗМЕРЫ:</h3>
          <div class="size-grid product-size-grid">
            <button
              v-for="sz in euSizes"
              :key="sz"
              class="size-option"
              :class="{ disabled: !availableSizeMap[sz], active: selectedSize?.size === sz }"
              @click="selectSize(sz)"
            >
              <span class="size-text">{{ sz }}</span>
              <span class="size-price" v-if="availableSizeMap[sz]">{{ formatPrice(availableSizeMap[sz].price) }}</span>
            </button>
          </div>
        </div>

        <div class="buy-block">
          <div class="buy-info">
            <div class="price-amount">{{ formatPrice(selectedSize ? selectedSize.price : product?.price) }}</div>
            <div class="size-selected" v-if="selectedSize">РАЗМЕР - {{ selectedSize.size }}</div>
          </div>
          <button class="add-to-cart-button" :disabled="!selectedSize" @click="addToCart">
            <span>ДОБАВИТЬ В КОРЗИНУ</span>
            <img class="btn-arrow" src="../assets/images/icons/arrow_right.svg" alt="">
          </button>
        </div>

      </div>
    </div>

    <div class="tabs-section">
      <div class="tabs">
        <button :class="['tab', { active: activeTab==='details' }]" @click="activeTab='details'">Детали</button>
        <button :class="['tab', { active: activeTab==='faq' }]" @click="activeTab='faq'">FAQ</button>
      </div>
      <div class="tab-content">
        <div v-if="activeTab==='details'" class="details">
          <ul class="details-list">
            <li><span class="label">Артикул</span><span class="value">{{ product.id }}</span></li>
            <li><span class="label">Категория</span><span class="value">{{ product.category }}</span></li>
            <li><span class="label">Бренд</span><span class="value">{{ product.brand }}</span></li>
            <li><span class="label">Модель</span><span class="value">{{ product.model }}</span></li>
          </ul>
        </div>
        <div v-else class="faq">
          <div class="faq-item">
            <div class="q">Как выбрать размер?</div>
            <div class="a">Выберите EU размер, активные варианты соответствуют наличию товара</div>
          </div>
          <div class="faq-item">
            <div class="q">Можно ли вернуть товар?</div>
            <div class="a">Да, в течение 14 дней при сохранении товарного вида.</div>
          </div>
        </div>
      </div>
    </div>

    <div class="offers">
      <h2 class="offers-title">ИНТЕРЕСНЫЕ ПРЕДЛОЖЕНИЯ</h2>
      <div class="offers-grid">
        <ProductCard
          v-for="p in interesting.slice(0,4)"
          :key="p.id"
          :product="p"
          @click="goToProduct(p.id)"
        />
      </div>
    </div>
  </section>

  <section v-else class="product-page loading-state">
    <div class="loading">Загрузка товара...</div>
  </section>
</template>

<script>
import ProductCard from '../components/ProductCard.vue'
import { showToast } from '../utils/toast'

export default {
  name: 'ProductPage',
  components: { ProductCard },
  data() {
    return {
      product: null,
      loaded: false,
      selectedImage: '',
      selectedSize: null,
      activeTab: 'details',
      interesting: [],
      euSizes: ['36','36.5','37','37.5','38','38.5','39','39.5','40','40.5','41','41.5','42','42.5','43','43.5','44','44.5','45']
    }
  },
  computed: {
    availableSizeMap() {
      const map = {}
      const sizes = Array.isArray(this.product?.sizes) ? this.product.sizes : []
      sizes.forEach(s => {
        const key = String(s.size)
        map[key] = { size: key, price: Number(s.price) }
      })
      return map
    }
  },
  async mounted() {
    await this.fetchProduct()
    await this.fetchInteresting()
  },
  watch: {
    '$route.params.id': {
      async handler() {
        await this.fetchProduct()
        await this.fetchInteresting()
      }
    }
  },
  methods: {
    async fetchProduct() {
      this.loaded = false
      const id = Number(this.$route.params.id)
      try {
        const res = await fetch(`/api/products/${id}`)
        if (!res.ok) throw new Error('not_found')
        this.product = await res.json()
        this.selectedImage = this.product.mainImageUrl || (this.product.images?.[0] ?? '')
        const sizes = Object.values(this.availableSizeMap)
        if (sizes.length) {
          this.selectedSize = sizes.reduce((min, s) => (min.price <= s.price ? min : s), sizes[0])
        } else {
          this.selectedSize = null
        }
      } catch (e) {
        this.product = null
      } finally {
        this.loaded = true
      }
    },
    async fetchInteresting() {
      try {
        const res = await fetch('/api/products')
        if (res.ok) {
          const list = await res.json()
          const sameCategory = list.filter(p => p.category === this.product?.category && p.id !== this.product?.id)
          this.interesting = sameCategory.length ? sameCategory : list.filter(p => p.id !== this.product?.id)
        }
      } catch (e) {
        console.error('Не удалось загрузить интересные предложения', e)
      }
    },
    selectSize(sz) {
      const s = this.availableSizeMap[sz]
      if (!s) return
      this.selectedSize = s
    },
    formatPrice(n) {
      const v = Math.round(Number(n) || 0)
      try { return new Intl.NumberFormat('ru-RU').format(v) + ' ₽' } catch (_) { return v + ' ₽' }
    },
    addToCart() {
      if (!this.selectedSize) return
      showToast('success', `Размер ${this.selectedSize.size} добавлен в корзину`)
    },
    goToProduct(id) {
      this.$router.push({ name: 'Product', params: { id } })
    },
    goBack() {
      // Вернуться на предыдущую страницу, если она есть; иначе в каталог
      if (window.history && window.history.length > 1) {
        this.$router.back()
      } else {
        this.$router.push({ name: 'Catalog' })
      }
    }
  }
}
</script>