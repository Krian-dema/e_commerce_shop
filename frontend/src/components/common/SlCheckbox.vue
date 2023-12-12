<script setup>
defineProps({
  checkboxId: {
    type: String,
    required: true,
  },
  styleClass: String,
});

defineEmits({
  changeValue: null,
});
</script>

<template>
  <div class="flex items-center">
    <input
      :id="checkboxId"
      :class="styleClass"
      type="checkbox"
      class="border-2 border-primary-500 text-primary-500 rounded hover:border-primary-800 focus:ring-primary-700 hover:cursor-pointer"
      @input="$emit('changeValue', $event.target.checked)"
    />
    <label v-if="$slots.default" :for="checkboxId" class="ml-4"><slot></slot></label>
  </div>
</template>
