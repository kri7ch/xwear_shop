<template>
  <div class="card-product">
    <div class="img-product">
      <img :src="product.mainImageUrl" :alt="product.name">
    </div>
    <div class="block-name-price">
      <p class="name-product">{{ product.name }}</p>
      <p class="price-product">от {{ priceText }}</p>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ProductCard',
  props: {
    product: { type: Object, required: true }
  },
  computed: {
    minPrice() {
      const sizes = this.product?.sizes || []
      if (!sizes.length) return 0
      return sizes.reduce((min, s) => Math.min(min, s.price), sizes[0].price)
    },
    priceText() {
      const n = Math.round(this.minPrice)
      try {
        return new Intl.NumberFormat('ru-RU').format(n) + ' ₽'
      } catch (_) {
        return n + ' ₽'
      }
    }
  }
}
</script>