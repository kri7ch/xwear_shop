<template>
    <div class="block-add-catalog">
        <div class="block-image-catalog">
            <img src="../assets/images/add/add_main_top.svg" alt="">
        </div>
        
        <div class="block-text-button-catalog">
            <h1>
                ШИРОКИЙ<br>
                АССОРТИМЕНТ<br>
                ОДЕЖДЫ
            </h1>

            <p class="text-in-add">
                Одежда от известные брендов у нас в каталоге.<br>
                Только качественные вещи.
            </p>

            <button class="btn-catalog-add" @click="goToCatalog">
                <p>ПЕРЕЙТИ В КАТАЛОГ</p>
                <img src="../assets/images/icons/arrow_right.svg" alt="">
            </button>
        </div>
    </div>

    <div class="block-shoes">
        <div class="block-top-content-block">
            <h1 class="tag-catalog">
                ОБУВЬ
            </h1>

            <div class="link-more-products" @click="goToCategoryShoes">
                <p>БОЛЬШЕ ТОВАРОВ</p>
                <img src="../assets/images/icons/arrow_right_black.svg" alt="">
            </div>
        </div>

        <div class="block-shoes-buy">
            <ProductCard v-for="p in shoesProducts.slice(0,4)" :key="p.id" :product="p" />
        </div>
    </div>

    <div class="block-shoes">
        <div class="block-top-content-block">
            <h1 class="tag-catalog">
                ОДЕЖДА
            </h1>

            <div class="link-more-products" @click="goToCategoryClothes">
                <p>БОЛЬШЕ ТОВАРОВ</p>
                <img src="../assets/images/icons/arrow_right_black.svg" alt="">
            </div>
        </div>

        <div class="block-shoes-buy">
            <ProductCard v-for="p in clothesProducts.slice(0,4)" :key="p.id" :product="p" />
        </div>
    </div>

    <div class="block-shoes">
        <div class="block-top-content-block">
            <h1 class="tag-catalog">
                АКСЕССУАРЫ
            </h1>

            <div class="link-more-products" @click="goToCategoryAccessories">
                <p>БОЛЬШЕ ТОВАРОВ</p>
                <img src="../assets/images/icons/arrow_right_black.svg" alt="">
            </div>
        </div>

        <div class="block-shoes-buy">
            <ProductCard v-for="p in accessoriesProducts.slice(0,4)" :key="p.id" :product="p" />
        </div>
    </div>
</template>

<script>
import ProductCard from '../components/ProductCard.vue'

export default {
  name: 'AppMainPage',
  components: { ProductCard },
  data() {
    return {
      products: []
    }
  },
  async mounted() {
    try {
      const res = await fetch('/api/products')
      if (!res.ok) throw new Error('failed')
      this.products = await res.json()
    } catch (e) {
      console.error('Не удалось загрузить товары', e)
    }
  },
  computed: {
    shoesProducts() {
      const cats = ['Sneakers','Boots','Sandals','Slippers','Shoes']
      return this.products.filter(p => cats.includes(p.category))
    },
    clothesProducts() {
      const cats = ['Sweaters','Shirts']
      return this.products.filter(p => cats.includes(p.category))
    },
    accessoriesProducts() {
      const cats = ['Hats','Caps','Bags']
      return this.products.filter(p => cats.includes(p.category))
    }
  },
  methods: {
    goToCatalog() {
      this.$router.push('/catalog')
    },
    goToCategoryShoes() { this.goToCatalog() },
    goToCategoryClothes() { this.goToCatalog() },
    goToCategoryAccessories() { this.goToCatalog() }
  }
}
</script>