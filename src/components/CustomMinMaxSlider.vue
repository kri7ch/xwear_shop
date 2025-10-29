<script>
export default {
  name: 'CustomMinMaxSlider',
  props: {
    min: { type: Number, default: 0 },
    max: { type: Number, default: 100 },
    step: { type: Number, default: 1 },
    minValue: { type: Number, default: 0 },
    maxValue: { type: Number, default: 100 }
  },
  emits: ['update:minValue', 'update:maxValue'],
  data() {
    return {
      sliderMinValue: this.minValue,
      sliderMaxValue: this.maxValue
    }
  },
  computed: {
    sliderDifference() {
      return Math.abs(this.sliderMaxValue - this.sliderMinValue)
    }
  },
  methods: {
    clamp(val, min, max) {
      return Math.min(Math.max(val, min), max)
    },
    getPercent(value, min, max) {
      if (max === min) return 0
      return ((value - min) / (max - min)) * 100
    },
    setCSSProps(width, left, right) {
      const el = this.$refs.slider
      if (!el) return
      el.style.setProperty('--width', `${width}%`)
      el.style.setProperty('--progressLeft', `${left}%`)
      el.style.setProperty('--progressRight', `${right}%`)
    },
    onMinInput(e) {
      const val = this.clamp(parseFloat(e.target.value), this.min, this.max)
      this.sliderMinValue = Math.min(val, this.sliderMaxValue)
    },
    onMaxInput(e) {
      const val = this.clamp(parseFloat(e.target.value), this.min, this.max)
      this.sliderMaxValue = Math.max(val, this.sliderMinValue)
    }
  },
  watch: {
    sliderMinValue(val) {
      this.$emit('update:minValue', val)
      const diff = this.getPercent(this.sliderDifference, this.min, this.max)
      const left = this.getPercent(val, this.min, this.max)
      const right = 100 - this.getPercent(this.sliderMaxValue, this.min, this.max)
      this.setCSSProps(diff, left, right)
    },
    sliderMaxValue(val) {
      this.$emit('update:maxValue', val)
      const diff = this.getPercent(this.sliderDifference, this.min, this.max)
      const left = this.getPercent(this.sliderMinValue, this.min, this.max)
      const right = 100 - this.getPercent(val, this.min, this.max)
      this.setCSSProps(diff, left, right)
    },
    minValue(nv) {
      this.sliderMinValue = this.clamp(nv, this.min, this.max)
      if (this.sliderMinValue > this.sliderMaxValue) {
        this.sliderMaxValue = this.sliderMinValue
      }
    },
    maxValue(nv) {
      this.sliderMaxValue = this.clamp(nv, this.min, this.max)
      if (this.sliderMaxValue < this.sliderMinValue) {
        this.sliderMinValue = this.sliderMaxValue
      }
    }
  },
  mounted() {
    const diff = this.getPercent(this.sliderDifference, this.min, this.max)
    const left = this.getPercent(this.sliderMinValue, this.min, this.max)
    const right = 100 - this.getPercent(this.sliderMaxValue, this.min, this.max)
    this.setCSSProps(diff, left, right)
  }
}
</script>

<template>
  <div ref="slider" class="custom-slider minmax">
    <input
      type="range"
      name="min"
      :min="min"
      :max="max"
      :step="step"
      :value="sliderMinValue"
      @input="onMinInput"
    />
    <input
      type="range"
      name="max"
      :min="min"
      :max="max"
      :step="step"
      :value="sliderMaxValue"
      @input="onMaxInput"
    />
  </div>
</template>