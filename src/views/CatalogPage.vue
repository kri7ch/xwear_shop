<template>
  <section class="catalog-page">
    <div class="catalog-container">
      <aside class="catalog-sidebar">
        <h2 class="sidebar-title">ФИЛЬТРЫ</h2>
        <div class="filter-section" :class="{ collapsed: collapsedSections.categories }">
          <h3 class="filter-title" @click="toggleSection('categories')">
            КАТЕГОРИИ
            <span class="collapse-toggle"></span>
          </h3>
          <ul class="filter-list scrollable" v-show="!collapsedSections.categories">
            <li class="filter-item" 
                v-for="category in categories" 
                :key="category.id"
                :class="{ active: selectedCategory === category.id }"
                @click="selectCategory(category.id)">
              {{ category.name }}
            </li>
          </ul>
        </div>

        <div class="filter-section" :class="{ collapsed: collapsedSections.price }">
          <h3 class="filter-title" @click="toggleSection('price')">
            ЦЕНА
            <span class="collapse-toggle"></span>
          </h3>
          <div class="price-range" v-show="!collapsedSections.price">
            <div class="price-inputs">
              <input type="number" v-model.number="priceFrom" placeholder="От" class="price-input" :min="minPrice" :max="maxPrice">
              <span class="price-separator">—</span>
              <input type="number" v-model.number="priceTo" placeholder="До" class="price-input" :min="minPrice" :max="maxPrice">
            </div>
            <div class="price-slider">
              <CustomMinMaxSlider
                :min="minPrice"
                :max="maxPrice"
                :step="100"
                v-model:minValue="priceFrom"
                v-model:maxValue="priceTo"
              />
            </div>
          </div>
        </div>

        <div class="filter-section" :class="{ collapsed: collapsedSections.sizes }">
          <h3 class="filter-title" @click="toggleSection('sizes')">
            РАЗМЕРЫ
            <span class="collapse-toggle"></span>
          </h3>
          <div class="size-grid scrollable-grid" v-show="!collapsedSections.sizes">
            <button v-for="size in availableSizes" 
                    :key="size"
                    :class="{ active: selectedSizes.includes(size) }"
                    @click="toggleSize(size)"
                    class="size-button">
              {{ size }}
            </button>
          </div>
        </div>

        <div class="filter-section" :class="{ collapsed: collapsedSections.brands }">
          <h3 class="filter-title" @click="toggleSection('brands')">
            БРЕНДЫ
            <span class="collapse-toggle"></span>
          </h3>
          <ul class="filter-list scrollable" v-show="!collapsedSections.brands">
            <li class="filter-item checkbox-item" v-for="brand in brands" :key="brand.id">
              <label class="checkbox-label">
                <input type="checkbox" 
                       :value="brand.id" 
                       v-model="selectedBrands"
                       class="checkbox-input">
                <span class="checkbox-custom"></span>
                {{ brand.name }}
              </label>
            </li>
          </ul>
        </div>

        <div class="filter-section" :class="{ collapsed: collapsedSections.models }">
          <h3 class="filter-title" @click="toggleSection('models')">
            МОДЕЛИ
            <span class="collapse-toggle"></span>
          </h3>
          <ul class="filter-list scrollable" v-show="!collapsedSections.models">
            <li class="filter-item checkbox-item" v-for="model in models" :key="model.id">
              <label class="checkbox-label">
                <input type="checkbox" 
                       :value="model.id" 
                       v-model="selectedModels"
                       class="checkbox-input">
                <span class="checkbox-custom"></span>
                {{ model.name }}
              </label>
            </li>
          </ul>
        </div>
      </aside>

      <main class="catalog-main">
        <div class="catalog-header">
          <h1 class="catalog-title">{{ currentCategoryName || 'КАТАЛОГ' }}</h1>
          <div class="catalog-controls">
            <div class="results-count">{{ filteredProducts.length }} товаров</div>
            <div class="sort-controls">
              <select v-model="sortBy" class="sort-select">
                <option value="default">По умолчанию</option>
                <option value="price-asc">Цена: по возрастанию</option>
                <option value="price-desc">Цена: по убыванию</option>
                <option value="name">По названию</option>
              </select>
            </div>
          </div>
        </div>

        <div class="products-grid">
          <ProductCard 
            v-for="product in paginatedProducts" 
            :key="product.id"
            :product="product"
            @click="goToProduct(product.id)"
          />
        </div>

        <div class="pagination" v-if="totalPages > 1">
          <button 
            v-for="page in totalPages" 
            :key="page"
            :class="{ active: currentPage === page }"
            @click="currentPage = page"
            class="pagination-button">
            {{ page }}
          </button>
        </div>
      </main>
    </div>
  </section>
</template>

<script>
import ProductCard from '../components/ProductCard.vue'
import CustomMinMaxSlider from '../components/CustomMinMaxSlider.vue'

export default {
  name: 'CatalogPage',
  components: {
    ProductCard,
    CustomMinMaxSlider
  },
  data() {
    return {
      selectedCategory: null,
      collapsedSections: {
        categories: false,
        price: false,
        sizes: false,
        brands: false,
        models: false
      },
      priceFrom: 0,
      priceTo: 50000,
      minPrice: 0,
      maxPrice: 50000,
      selectedSizes: [],
      selectedBrands: [],
      selectedModels: [],
      
      sortBy: 'default',
      currentPage: 1,
      itemsPerPage: 12,
      
      products: [],
      categories: [],
      brands: [],
      models: [],
      availableSizes: [],
      
      loading: false
    }
  },
  computed: {
    currentCategoryName() {
      const category = this.categories.find(c => c.id === this.selectedCategory)
      return category ? category.name : null
    },
    
    filteredProducts() {
      let filtered = [...this.products]
      
      const getMinPrice = (prod) => {
        const prices = Array.isArray(prod.sizes)
          ? prod.sizes.map(s => Number(s.price)).filter(n => !isNaN(n))
          : []
        return prices.length ? Math.min(...prices) : 0
      }
      
      if (this.selectedCategory) {
        const cat = this.categories.find(c => c.id === this.selectedCategory)
        const catName = cat ? cat.name : null
        if (catName) {
          filtered = filtered.filter(p => p.category === catName)
        }
      }
      
      filtered = filtered.filter(p => {
        const min = getMinPrice(p)
        return min >= this.priceFrom && min <= this.priceTo
      })
      
      if (this.selectedSizes.length > 0) {
        filtered = filtered.filter(p => {
          const sizes = Array.isArray(p.sizes) ? p.sizes.map(s => String(s.size)) : []
          return sizes.some(sz => this.selectedSizes.includes(sz))
        })
      }
      
      if (this.selectedBrands.length > 0) {
        const brandNames = this.brands
          .filter(b => this.selectedBrands.includes(b.id))
          .map(b => b.name)
        filtered = filtered.filter(p => brandNames.includes(p.brand))
      }
      
      if (this.selectedModels.length > 0) {
        const modelNames = this.models
          .filter(m => this.selectedModels.includes(m.id))
          .map(m => m.name)
        filtered = filtered.filter(p => modelNames.includes(p.model))
      }
      
      switch (this.sortBy) {
        case 'price-asc':
          filtered.sort((a, b) => getMinPrice(a) - getMinPrice(b))
          break
        case 'price-desc':
          filtered.sort((a, b) => getMinPrice(b) - getMinPrice(a))
          break
        case 'name':
          filtered.sort((a, b) => a.name.localeCompare(b.name))
          break
      }
      
      return filtered
    },
    paginatedProducts() {
      const start = (this.currentPage - 1) * this.itemsPerPage
      const end = start + this.itemsPerPage
      return this.filteredProducts.slice(start, end)
    },
    totalPages() {
      return Math.ceil(this.filteredProducts.length / this.itemsPerPage)
    }
  },
  async mounted() {
    await this.loadInitialData()
  },
  watch: {
    selectedBrands() { this.currentPage = 1 },
    selectedModels() { this.currentPage = 1 },
    priceFrom(val) {
      if (val > this.priceTo) this.priceTo = val
      if (val < this.minPrice) this.priceFrom = this.minPrice
      this.currentPage = 1
    },
    priceTo(val) {
      if (val < this.priceFrom) this.priceFrom = val
      if (val > this.maxPrice) this.priceTo = this.maxPrice
      this.currentPage = 1
    },
    sortBy() { this.currentPage = 1 }
  },
  methods: {
    toggleSection(section) {
      if (this.collapsedSections && Object.prototype.hasOwnProperty.call(this.collapsedSections, section)) {
        this.collapsedSections[section] = !this.collapsedSections[section]
      }
    },
    async loadInitialData() {
      this.loading = true
      try {
        await Promise.all([
          this.loadProducts(),
          this.loadCategories(),
          this.loadBrands(),
          this.loadModels(),
          this.loadSizes()
        ])
      } catch (error) {
        console.error('Ошибка загрузки данных каталога:', error)
      } finally {
        this.loading = false
      }
    },
    
    async loadProducts() {
      try {
        const response = await fetch('/api/products')
        if (response.ok) {
          this.products = await response.json()
        }
      } catch (error) {
        console.error('Ошибка загрузки товаров:', error)
      }
    },
    
    async loadCategories() {
      try {
        const response = await fetch('/api/categories')
        if (response.ok) {
          this.categories = await response.json()
        }
      } catch (error) {
        console.error('Ошибка загрузки категорий:', error)
      }
    },
    
    async loadBrands() {
      try {
        const response = await fetch('/api/brands')
        if (response.ok) {
          this.brands = await response.json()
        }
      } catch (error) {
        console.error('Ошибка загрузки брендов:', error)
      }
    },
    
    async loadModels() {
      try {
        const response = await fetch('/api/models')
        if (response.ok) {
          this.models = await response.json()
        }
      } catch (error) {
        console.error('Ошибка загрузки моделей:', error)
      }
    },
    
    async loadSizes() {
      this.availableSizes = ['36', '37', '38', '39', '40', '41', '42', '43', '44', '45', '46']
    },
    
    selectCategory(categoryId) {
      if (this.selectedCategory === categoryId) {
        this.selectedCategory = null
      } else {
        this.selectedCategory = categoryId
      }
      this.currentPage = 1
    },
    
    toggleSize(size) {
      const index = this.selectedSizes.indexOf(size)
      if (index > -1) {
        this.selectedSizes.splice(index, 1)
      } else {
        this.selectedSizes.push(size)
      }
      this.currentPage = 1
    },
    
    goToProduct(productId) {
      this.$router.push({ name: 'Product', params: { id: productId } })
    }
  }
}
</script>