export default {
    data: () => ({
        filter: "",
    }),
    computed: {
        filteredTricks()
        {
            if (!this.filter) return this.tricks
        
            const normalizedFilter = this.filter.trim().toLowerCase();
        
            return this.tricks.filter(t => t.name.toLowerCase().includes(normalizedFilter)
                || t.description.toLowerCase().includes(normalizedFilter))
        
        }
    }
}