import { defineStore } from 'pinia';

export const useWishlistStore = defineStore('wishlist', {
  state: () => ({
    wishlist: [], // Guarda los IDs de productos en la lista
  }),
  actions: {
    toggleWishlist(producto) {
      const index = this.wishlist.findIndex(p => p.idProducto === producto.idProducto);
      if (index !== -1) {
        // Ya estaba, así que lo eliminamos
        this.wishlist.splice(index, 1);
      } else {
        // Lo agregamos
        this.wishlist.push(producto);
      }
    },
    isInWishlist(idProducto) {
      return this.wishlist.some(p => p.idProducto === idProducto);
    },
    clearWishlist() {
      this.wishlist = [];
    },
  },
  persist: true, // Opcional, si quieres que la lista se guarde en LocalStorage
});