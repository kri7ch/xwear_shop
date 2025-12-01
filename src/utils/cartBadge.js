let subscribers = []

export function subscribeCartCount(handler) {
  if (typeof handler !== 'function') return () => {}
  subscribers.push(handler)
  return () => {
    subscribers = subscribers.filter(h => h !== handler)
  }
}

export function setCartCount(count) {
  const n = Math.max(0, Number(count) || 0)
  subscribers.forEach(h => {
    try { 
      h(n)
    } catch (e) {
      console.warn('Ошибка обновления сигнала', e)
    }
  })
}
