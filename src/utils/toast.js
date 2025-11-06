let subscribers = [];

export function showToast(type, message, timeout = 2000) {
  const toast = { id: Date.now() + Math.random(), type, message, timeout };
  subscribers.forEach(fn => fn(toast));
}

export function subscribeToast(fn) {
  subscribers.push(fn);
  return () => {
    subscribers = subscribers.filter(s => s !== fn);
  };
}